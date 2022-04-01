using System;
using System.Windows.Input;

namespace Update
{
    public sealed class DelegateCommand : ICommand
    {
        /// Member Variables
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        public event EventHandler CanExecuteChanged;
        public DelegateCommand(Action<object> execute) : this(execute, null) { } /// Constructor
		public DelegateCommand(Action<object> execute, Predicate<object> canExecute) /// Constructor
		{
            this._execute = execute;
            this._canExecute = canExecute;
        }
        /// Member Functions
        public bool CanExecute(object parameter) => (this._canExecute == null || this._canExecute(parameter));
        public void Execute(object parameter) => this._execute(parameter);
        public void RaiseCanExecuteChanged()
        {
            EventHandler canExecuteChanged = this.CanExecuteChanged;
            if (canExecuteChanged == null) return;
            canExecuteChanged(this, EventArgs.Empty);
        }
    }
}
