// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyPropertyChangedInvocatorAttribute.cs" company="James Croft">
//   Copyright (C) James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the NotifyPropertyChangedInvocatorAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinPhoneControls.Common.Attributes
{
    using System;

    /// <summary>
    /// The notify property changed invocator attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedInvocatorAttribute"/> class.
        /// </summary>
        public NotifyPropertyChangedInvocatorAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedInvocatorAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">
        /// The parameter name.
        /// </param>
        public NotifyPropertyChangedInvocatorAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string ParameterName { get; private set; }
    }
}
