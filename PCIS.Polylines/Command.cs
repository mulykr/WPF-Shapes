using System;
using System.Windows.Input;

namespace Polylines
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class
        /// </summary>
        /// <param name="execute">Executing action</param>
        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class
        /// </summary>
        /// <param name="execute">Executing action</param>
        /// <param name="canExecute">Predicate that checks if action is executable</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        /// <summary>
        /// Event of changing executable action
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// Default predicate
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        /// <summary>
        /// Exeute command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        /// <summary>
        /// Handler for changing executing command
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Destroys command
        /// </summary>
        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        /// <summary>
        /// Default predicate
        /// </summary>
        /// <param name="parameter">Parameter of the method (non-usable)</param>
        /// <returns></returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
