using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace tfmStandalone
{
	public sealed class CustomCommandViewModel : BindableBase
	{
		private string _command;
		private string _validationError;
		public ObservableCollection<CustomCommandStepViewModel> Steps { get; }
		public DelegateCommand AddStepCommand { get; }
		public DelegateCommand<CustomCommandStepViewModel> RemoveStepCommand { get; }
		public string Command
		{
			get
			{
				return this._command;
			}
			set
			{
				this.SetProperty<string>(ref this._command, value, "Command");
			}
		}
		public string ValidationError
		{
			get
			{
				return this._validationError;
			}
			set
			{
				this.SetProperty<string>(ref this._validationError, value, "ValidationError");
			}
		}
		public CustomCommandViewModel()
		{
			this.Steps = new ObservableCollection<CustomCommandStepViewModel>();
			this.AddStepCommand = new DelegateCommand(delegate() {this.Steps.Add(new CustomCommandStepViewModel());});
			this.RemoveStepCommand = new DelegateCommand<CustomCommandStepViewModel>(delegate(CustomCommandStepViewModel step) { if (this.Steps.Contains(step)){this.Steps.Remove(step);}});
		}
	}
}
