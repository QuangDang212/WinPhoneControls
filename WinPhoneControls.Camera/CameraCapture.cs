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
    using System.Runtime.CompilerServices;

    using WinPhoneControls.Common.Attributes;

    /// <summary>
    /// The camera capture.
    /// </summary>
    public class CameraCapture : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// The property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Disposes the CameraCapture.
        /// </summary>
        public void Dispose()
        {
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