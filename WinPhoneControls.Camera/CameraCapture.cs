// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCapture.cs" company="James Croft">
//   Copyright (C) James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the CameraCapture type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinPhoneControls.Camera
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Windows.Devices.Enumeration;
    using Windows.Media.Capture;
    using Windows.Media.Devices;
    using Windows.Media.MediaProperties;
    using Windows.Storage;

    using WinPhoneControls.Common.Attributes;

    /// <summary>
    /// The camera capture.
    /// </summary>
    public class CameraCapture : INotifyPropertyChanged, IDisposable
    {
        private MediaCapture _mediaCapture;

        private IMediaEncodingProperties _mediaEncodingProperties;

        private ImageEncodingProperties _imgEncodingProperties;

        private MediaEncodingProfile _videoEncodingProperties;

        /// <summary>
        /// The property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes the camera capture.
        /// </summary>
        /// <param name="captureUse">
        /// The capture use.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<MediaCapture> Initialize(CaptureUse captureUse)
        {
            if (this._mediaCapture != null)
            {
                this.Dispose();
            }

            var camera = await GetDeviceInfo(Panel.Back);

            this._mediaCapture = new MediaCapture();

            this._mediaCapture.Failed += this.OnCaptureFailed;

            await
                this._mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = camera.Id });

            this._mediaCapture.VideoDeviceController.PrimaryUse = captureUse;

            this._imgEncodingProperties = ImageEncodingProperties.CreateJpeg();
            this._videoEncodingProperties = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto);

            if (captureUse == CaptureUse.Photo)
            {
                // get all possible resolutions for the current controller
                var resolutions = this._mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo)
                        .ToList();

                var max = 0;
                foreach (
                    var res in resolutions.OfType<VideoEncodingProperties>().Where(res => res.Width * res.Height > max))
                {
                    // find the maximum resolution possible
                    max = (int)(res.Width * res.Height);
                    this._mediaEncodingProperties = res;

                    this._imgEncodingProperties.Width = res.Width;
                    this._imgEncodingProperties.Height = res.Height;
                }

                await
                    this._mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(
                        MediaStreamType.Photo,
                        this._mediaEncodingProperties);
            }

            return this._mediaCapture;
        }

        /// <summary>
        /// Captures a photo to temporary storage.
        /// </summary>
        /// <returns>
        /// The <see cref="StorageFile"/>.
        /// </returns>
        public async Task<StorageFile> CapturePhoto()
        {
            var file = await CreateTempFile(".jpg");

            try
            {
                await this._mediaCapture.CapturePhotoToStorageFileAsync(this._imgEncodingProperties, file);
            }
            catch (Exception)
            {
                if (file == null)
                {
                    return null;
                }
            }

            return file;
        }

        /// <summary>
        /// Disposes the CameraCapture.
        /// </summary>
        public void Dispose()
        {
            if (this._mediaCapture == null)
            {
                return;
            }

            this._mediaCapture.Dispose();
            this._mediaCapture = null;
        }

        private static async Task<DeviceInformation> GetDeviceInfo(Panel desiredPanel)
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            var camera =
                devices.FirstOrDefault(
                    info => info.EnclosureLocation != null && info.EnclosureLocation.Panel == desiredPanel);

            if (camera != null) return camera;

            throw new Exception(string.Format("Camera of type {0} doesn't exist.", desiredPanel));
        }

        private static async Task<StorageFile> CreateTempFile(string ext)
        {
            var tempFolder = ApplicationData.Current.TemporaryFolder;
            return await tempFolder.CreateFileAsync(string.Format("{0}{1}", Guid.NewGuid(), ext));
        }

        private void OnCaptureFailed(MediaCapture sender, MediaCaptureFailedEventArgs e)
        {
            this.Dispose();
            this._mediaCapture.Failed -= this.OnCaptureFailed;
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}