// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureControl.xaml.cs" company="James Croft">
//   Copyright (C) James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the CameraCaptureControl type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinPhoneControls.Camera
{
    using System.Windows.Input;

    using WinPhoneControls.Common.Commands;

    /// <summary>
    /// The camera capture control.
    /// </summary>
    public sealed partial class CameraCaptureControl
    {
        private readonly ICommand _capturePhotoCommand;

        private readonly CameraCapture _cameraCapture;

        /// <summary>
        /// Initialises a new instance of the <see cref="CameraCaptureControl"/> class.
        /// </summary>
        public CameraCaptureControl()
        {
            this.InitializeComponent();

            this._cameraCapture = new CameraCapture();
            this._capturePhotoCommand = new RelayCommand(this.CapturePhoto);
        }

        /// <summary>
        /// Gets the capture photo command.
        /// </summary>
        public ICommand CapturePhotoCommand
        {
            get
            {
                return this._capturePhotoCommand;
            }
        }

        private async void CapturePhoto()
        {
            var photo = await this._cameraCapture.CapturePhoto();
        }
    }
}