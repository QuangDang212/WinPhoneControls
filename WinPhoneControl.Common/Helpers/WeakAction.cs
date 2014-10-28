// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakAction.cs" company="James Croft">
//   Copyright (C) James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the WeakAction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinPhoneControls.Common.Helpers
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents a weak action. 
    /// </summary>
    public class WeakAction
    {
        private readonly Action _action;

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakAction"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public WeakAction(Action action)
            : this(action == null ? null : action.Target, action)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakAction"/> class.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public WeakAction(object target, Action action)
        {
            if (action.GetMethodInfo().IsStatic)
            {
                this._action = action;
                if (target == null) return;
                this.Reference = new WeakReference(target);
            }
            else
            {
                this.Method = action.GetMethodInfo();
                this.ActionReference = new WeakReference(action.Target);
                this.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakAction"/> class.
        /// </summary>
        protected WeakAction()
        {
        }

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public virtual string MethodName
        {
            get
            {
                return this._action != null ? this._action.GetMethodInfo().Name : this.Method.Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is alive.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (this._action == null && this.Reference == null)
                {
                    return false;
                }

                if (this._action != null && this.Reference == null)
                {
                    return true;
                }

                return this.Reference.IsAlive;
            }
        }

        /// <summary>
        /// Gets the action's owner.
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
                return this._action != null;
            }
        }

        /// <summary>
        /// Gets the target of the weak reference.
        /// </summary>
        protected object ActionTarget
        {
            get
            {
                return this.ActionReference == null ? null : this.ActionReference.Target;
            }
        }

        /// <summary>
        /// Gets or sets the method info of the WeakAction method.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        /// Gets or sets the action's reference.
        /// </summary>
        protected WeakReference ActionReference { get; set; }

        /// <summary>
        /// Gets or sets the reference to the target passed in construction.
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public void Execute()
        {
            if (this._action != null)
            {
                this._action();
            }
            else
            {
                var target = this.ActionTarget;

                if (!this.IsAlive || this.Method == null || (this.ActionReference == null || target == null))
                {
                    return;
                }

                this.Method.Invoke(target, null);
            }
        }
    }

    /// <summary>
    /// The weak action.
    /// </summary>
    /// <typeparam name="T">
    /// The type of result stored. 
    /// </typeparam>
    public class WeakAction<T> : WeakAction
    {
        private readonly Action<T> _staticAction;

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakAction{T}"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public WeakAction(Action<T> action)
            : this(action == null ? null : action.Target, action)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakAction{T}"/> class.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public WeakAction(object target, Action<T> action)
        {
            if (action.GetMethodInfo().IsStatic)
            {
                this._staticAction = action;
                if (target == null) return;
                this.Reference = new WeakReference(target);
            }
            else
            {
                this.Method = action.GetMethodInfo();
                this.ActionReference = new WeakReference(action.Target);
                this.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public override string MethodName
        {
            get
            {
                return this._staticAction != null ? this._staticAction.GetMethodInfo().Name : this.Method.Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is alive.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if (this._staticAction == null && this.Reference == null)
                {
                    return false;
                }

                if (this._staticAction == null)
                {
                    return this.Reference.IsAlive;
                }

                return this.Reference == null || this.Reference.IsAlive;
            }
        }

        /// <summary>
        /// Executres the action.
        /// </summary>
        public new void Execute()
        {
            this.Execute(default(T));
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(T parameter)
        {
            if (this._staticAction != null)
            {
                this._staticAction(parameter);
            }
            else
            {
                var actionTarget = this.ActionTarget;

                if (!this.IsAlive || this.Method == null || (this.ActionReference == null || actionTarget == null))
                {
                    return;
                }

                this.Method.Invoke(actionTarget, new object[] { parameter });
            }
        }

        /// <summary>
        /// Executes the action with a parameter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void ExecuteWithObject(object parameter)
        {
            this.Execute((T)parameter);
        }
    }
}