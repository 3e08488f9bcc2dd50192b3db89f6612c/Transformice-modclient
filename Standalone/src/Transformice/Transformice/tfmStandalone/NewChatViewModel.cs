using System;
using Prism.Commands;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public class NewChatViewModel : BindableBase
	{
		public DelegateCommand OkCommand { get; }
		public DelegateCommand CancelCommand { get; }
		public EventHandler Accepted;
		public EventHandler Cancelled;
		private string _name;
		
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this.SetProperty<string>(ref this._name, value, "Name");
			}
		}
		
		public NewChatViewModel()
		{
			this.OkCommand = new DelegateCommand(delegate()
			{
				EventHandler accepted = this.Accepted;
				if (accepted == null)
				{
					return;
				}
				accepted(this, new EventArgs());
			});
			this.CancelCommand = new DelegateCommand(delegate()
			{
				EventHandler cancelled = this.Cancelled;
				if (cancelled == null)
				{
					return;
				}
				cancelled(this, new EventArgs());
			});
		}
	}
}
