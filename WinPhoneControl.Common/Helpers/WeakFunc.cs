// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakFunc.cs" company="James Croft">
//   Copyright (C) James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the WeakFunc type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinPhoneControls.Common.Helpers
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents a weak function.
    /// </summary>
    /// <typeparam name="T">
    /// The type of result stored.
    /// </typeparam>
    public class WeakFunc<T>
    {
        private readonly Func<T> _func;

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakFunc{T}"/> class.
        /// </summary>
        /// <param name="func">
        /// The func.
        /// </param>
        public WeakFunc(Func<T> func)
            : this(func == null ? null : func.Target, func)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakFunc{T}"/> class.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="func">
        /// The func.
        /// </param>
        public WeakFunc(object target, Func<T> func)
        {
            if (func.GetMethodInfo().IsStatic)
            {
                this._func = func;

                if (target == null)
                {
                    return;
                }

                this.Reference = new WeakReference(target);
            }
            else
            {
                this.Method = func.GetMethodInfo();
                this.FuncReference = new WeakReference(func.Target);
                this.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakFunc{T}"/> class.
        /// </summary>
        protected WeakFunc()
        {
        }

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public virtual string MethodName
        {
            get
            {
                return this._func != null ? this._func.GetMethodInfo().Name : this.Method.Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is alive.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (this._func == null && this.Reference == null)
                {
                    return false;
                }

                if (this._func != null && this.Reference == null)
                {
                    return true;
                }

                return this.Reference.IsAlive;
            }
        }

        /// <summary>
        /// Gets the func's owner.
        /// </summary>
        public object Target
        {
            get
            {
                return this.Reference == null ? null : this.Reference.Target;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is static.
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return this._func != null;
            }
        }

        /// <summary>
        /// Gets the target of the weak reference.
        /// </summary>
        protected object FuncTarget
        {
            get
            {
                return this.FuncReference == null ? null : this.FuncReference.Target;
            }
        }

        /// <summary>
        /// Gets or sets the method info of the WeakAction method.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        /// Gets or sets the func's reference
        /// </summary>
        protected WeakReference FuncReference { get; set; }

        /// <summary>
        /// Gets or sets the reference to the target passed in construction
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        /// Executes the func.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Execute()
        {
            if (this._func != null)
            {
                return this._func();
            }

            var funcTarget = this.FuncTarget;

            if (this.IsAlive && this.Method != null && (this.FuncReference != null && funcTarget != null))
            {
                return (T)this.Method.Invoke(funcTarget, null);
            }

            return default(T);
        }
    }
}