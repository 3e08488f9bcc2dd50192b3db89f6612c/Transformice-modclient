using System;
using System.IO;
using Prism.Commands;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class ChatNotesWindowViewModel : BindableBase
	{
		public DelegateCommand SaveCommand { get; }
		public EventHandler Closed;
		private string _name;
		private string _notes;
		
		public ChatNotesWindowViewModel(string name)
		{
			this.Name = name;
			string notesFile = AppDomain.CurrentDomain.BaseDirectory + "User Notes\\" + name + ".txt";
			this.SaveCommand = new DelegateCommand(delegate()
			{
				File.WriteAllText(notesFile, this.Notes);
				EventHandler closed = this.Closed;
				if (closed == null)
				{
					return;
				}
				closed(this, new EventArgs());
			});
			if (File.Exists(notesFile))
			{
				this.Notes = File.ReadAllText(notesFile);
			}
		}
		
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

		public string Notes
		{
			get
			{
				return this._notes;
			}
			set
			{
				this.SetProperty<string>(ref this._notes, value, "Notes");
			}
		}
	}
}
