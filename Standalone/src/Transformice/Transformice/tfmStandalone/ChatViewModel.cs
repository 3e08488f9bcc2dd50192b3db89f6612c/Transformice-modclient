using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public class ChatViewModel : BindableBase
	{
		public string Name { get; set; }
		public string TabColor { get; set; }
		public string TabNameColor { get; set; }
		public bool IsWhisper { get; set; }
		public ObservableCollection<ChatMessageViewModel> ChatMessages { get; set; }
		public DelegateCommand LogCommand { get; }
		public DelegateCommand CasierCommand { get; }
		public DelegateCommand JoinCommand { get; }
		public DelegateCommand IgnoreCommand { get; }
		public DelegateCommand MumuteCommand { get; }
		public DelegateCommand KickCommand { get; }
		public DelegateCommand CloseAllWhispersCommand { get; }
		public DelegateCommand CloseAllReadWhispersCommand { get; }
		public DelegateCommand CloseChatCommand { get; }
		public DelegateCommand ToggleMuteChatCommand { get; }
		public DelegateCommand TogglePinnedCommand { get; }
		private bool _isPinned;
		private bool _isMuted;
		private bool _newMessage;
		private string _community;
		private bool _isPreviousSelectedWHisper;
		private bool _isSelected;
		
		public bool IsPinned
		{
			get
			{
				return this._isPinned;
			}
			set
			{
				this.SetProperty<bool>(ref this._isPinned, value, "IsPinned");
			}
		}

		public bool IsMuted
		{
			get
			{
				return this._isMuted;
			}
			set
			{
				this.SetProperty<bool>(ref this._isMuted, value, "IsMuted");
			}
		}

		public bool HasNewMessages
		{
			get
			{
				return this._newMessage;
			}
			set
			{
				this.SetProperty<bool>(ref this._newMessage, value, "HasNewMessages");
			}
		}

		public string Community
		{
			get
			{
				return this._community;
			}
			set
			{
				this.SetProperty<string>(ref this._community, value, "Community");
			}
		}

		public bool IsPreviousSelectedWhisper
		{
			get
			{
				return this._isPreviousSelectedWHisper;
			}
			set
			{
				this.SetProperty<bool>(ref this._isPreviousSelectedWHisper, value, "IsPreviousSelectedWhisper");
			}
		}

		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this.SetProperty<bool>(ref this._isSelected, value, "IsSelected");
			}
		}

		public ChatViewModel(ChatWindowViewModel chatWindowViewModel)
		{
			ChatViewModel chatviewmodel = this;
			this.ChatMessages = new ObservableCollection<ChatMessageViewModel>();
			this.LogCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.SendCommand("l " + chatviewmodel.Name);
			});
			this.CasierCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.SendCommand("casier " + chatviewmodel.Name);
			});
			this.JoinCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.SendCommand("join " + chatviewmodel.Name);
			});
			this.IgnoreCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.SendClientCommand("tignore " + chatviewmodel.Name);
			});
			this.MumuteCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.SendCommand("mumute " + chatviewmodel.Name);
			});
			this.KickCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.SendCommand("kick " + chatviewmodel.Name);
			});
			this.CloseAllWhispersCommand = new DelegateCommand(new Action(chatWindowViewModel.CloseAllWhispers));
			this.CloseAllReadWhispersCommand = new DelegateCommand(new Action(chatWindowViewModel.CloseAllReadWhispers));
			this.CloseChatCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.CloseChat(chatviewmodel.Name);
			});
			this.ToggleMuteChatCommand = new DelegateCommand(delegate()
			{
				this.IsMuted = !this.IsMuted;
			});
			this.TogglePinnedCommand = new DelegateCommand(delegate()
			{
				chatWindowViewModel.TogglePinChat(chatviewmodel.Name);
			});
		}

		public void AddMessage(string color, string message, bool showNewMessage)
		{
			this.ChatMessages.Add(new ChatMessageViewModel
			{
				Color = color,
				Message = message
			});
			if (!this.IsSelected && showNewMessage && !this.IsMuted)
			{
				this.HasNewMessages = true;
			}
		}

		public void AddHistory(string color, IEnumerable<string> messages)
		{
			foreach (string message in messages)
			{
				this.ChatMessages.Add(new ChatMessageViewModel
				{
					Color = color,
					Message = message
				});
			}
		}
	}
}
