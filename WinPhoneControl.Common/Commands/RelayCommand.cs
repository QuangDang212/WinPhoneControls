// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="James Croft">
//   Copyright (C) James Croft. All rights reserved.
// </copyright>
// <summary>
//   Defines the RelayCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinPhoneControls.Common.Commands
{
    using System;
    using System.Windows.Input;

    using WinPhoneControls.Common.Helpers;

    /// <summary>
    /// The relay command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly WeakAction _execute;

        private readonly WeakFunc<bool> _canExecute;

        /// <summary>
        /// Initialises a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public RelayCommand(Action action)
            : this(action, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="canExec">
        /// The can exec.
        /// </param>
        public RelayCommand(Action action, Func<bool> canExec)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this._execute = new WeakAction(action);

            if (canExec == null)
            {
                return;
            }

            this._canExecute = new WeakFunc<bool>(canExec);
        }

        /// <summary>
        /// The can execute changed event handler.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }

            if (this._canExecute.IsStatic || this._canExecute.IsAlive)
            {
                return this._canExecute.Execute();
            }

            return false;
        }

        /// <summary>
        /// Called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            if (!this.CanExecute(parameter) || this._execute == null
                || (!this._execute.IsStatic && !this._execute.IsAlive))
            {
                return;
            }

            this._execute.Execute();
        }
    }
}