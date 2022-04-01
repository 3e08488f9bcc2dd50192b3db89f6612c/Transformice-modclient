using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;

namespace tfmStandalone
{
	// Token: 0x02000064 RID: 100
	public sealed class ChatSettingsWindowViewModel : BindableBase
	{
		public ObservableCollection<int> FontSizes { get; }
		public DelegateCommand SaveCommand { get; set; }
		public EventHandler Closed;
		private bool _filterModoChat;
		private bool _alertModoChat;
		private bool _logModoChat;
		private bool _filterArbChat;
		private bool _alertArbChat;
		private bool _logArbChat;
		private bool _filterServeurMessages;
		private bool _alertServeurMessages;
		private bool _logServeurMessages;
		private bool _filterMapCrewChat;
		private bool _alertMapCrewChat;
		private bool _logMapCrewChat;
		private bool _filterLuaTeamChat;
		private bool _alertLuaTeamChat;
		private bool _logLuaTeamChat;
		private bool _filterFunCorpChat;
		private bool _alertFunCorpChat;
		private bool _logFunCorpChat;
		private bool _filterFashionSquadChat;
		private bool _alertFashionSquadChat;
		private bool _logFashionSquadChat;
		private bool _filterTribeChat;
		private bool _alertTribeChat;
		private bool _logTribeChat;
		private bool _filterWhispers;
		private bool _alertWhispers;
		private bool _logWhispers;
		private FontFamily _fontFamily;
		private int _fontSize;
		private GameSettings GameSettings { get; }
		public bool FilterModoChat
		{
			get
			{
				return this._filterModoChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterModoChat, value, "FilterModoChat");
			}
		}
		public bool AlertModoChat
		{
			get
			{
				return this._alertModoChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertModoChat, value, "AlertModoChat");
			}
		}
		public bool LogModoChat
		{
			get
			{
				return this._logModoChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logModoChat, value, "LogModoChat");
			}
		}
		public bool FilterArbChat
		{
			get
			{
				return this._filterArbChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterArbChat, value, "FilterArbChat");
			}
		}
		public bool AlertArbChat
		{
			get
			{
				return this._alertArbChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertArbChat, value, "AlertArbChat");
			}
		}
		public bool LogArbChat
		{
			get
			{
				return this._logArbChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logArbChat, value, "LogArbChat");
			}
		}
		public bool FilterServeurMessages
		{
			get
			{
				return this._filterServeurMessages;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterServeurMessages, value, "FilterServeurMessages");
			}
		}
		public bool AlertServeurMessages
		{
			get
			{
				return this._alertServeurMessages;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertServeurMessages, value, "AlertServeurMessages");
			}
		}
		public bool LogServeurMessages
		{
			get
			{
				return this._logServeurMessages;
			}
			set
			{
				this.SetProperty<bool>(ref this._logServeurMessages, value, "LogServeurMessages");
			}
		}
		public bool FilterMapCrewChat
		{
			get
			{
				return this._filterMapCrewChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterMapCrewChat, value, "FilterMapCrewChat");
			}
		}
		public bool AlertMapCrewChat
		{
			get
			{
				return this._alertMapCrewChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertMapCrewChat, value, "AlertMapCrewChat");
			}
		}
		public bool LogMapCrewChat
		{
			get
			{
				return this._logMapCrewChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logMapCrewChat, value, "LogMapCrewChat");
			}
		}
		public bool FilterLuaTeamChat
		{
			get
			{
				return this._filterLuaTeamChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterLuaTeamChat, value, "FilterLuaTeamChat");
			}
		}
		public bool AlertLuaTeamChat
		{
			get
			{
				return this._alertLuaTeamChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertLuaTeamChat, value, "AlertLuaTeamChat");
			}
		}
		public bool LogLuaTeamChat
		{
			get
			{
				return this._logLuaTeamChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logLuaTeamChat, value, "LogLuaTeamChat");
			}
		}
		public bool FilterFunCorpChat
		{
			get
			{
				return this._filterFunCorpChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterFunCorpChat, value, "FilterFunCorpChat");
			}
		}
		public bool AlertFunCorpChat
		{
			get
			{
				return this._alertFunCorpChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertFunCorpChat, value, "AlertFunCorpChat");
			}
		}
		public bool LogFunCorpChat
		{
			get
			{
				return this._logFunCorpChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logFunCorpChat, value, "LogFunCorpChat");
			}
		}
		public bool FilterFashionSquadChat
		{
			get
			{
				return this._filterFashionSquadChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterFashionSquadChat, value, "FilterFashionSquadChat");
			}
		}
		public bool AlertFashionSquadChat
		{
			get
			{
				return this._alertFashionSquadChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertFashionSquadChat, value, "AlertFashionSquadChat");
			}
		}
		public bool LogFashionSquadChat
		{
			get
			{
				return this._logFashionSquadChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logFashionSquadChat, value, "LogFashionSquadChat");
			}
		}
		public bool FilterTribeChat
		{
			get
			{
				return this._filterTribeChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterTribeChat, value, "FilterTribeChat");
			}
		}
		public bool AlertTribeChat
		{
			get
			{
				return this._alertTribeChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertTribeChat, value, "AlertTribeChat");
			}
		}
		public bool LogTribeChat
		{
			get
			{
				return this._logTribeChat;
			}
			set
			{
				this.SetProperty<bool>(ref this._logTribeChat, value, "LogTribeChat");
			}
		}
		public bool FilterWhispers
		{
			get
			{
				return this._filterWhispers;
			}
			set
			{
				this.SetProperty<bool>(ref this._filterWhispers, value, "FilterWhispers");
			}
		}
		public bool AlertWhispers
		{
			get
			{
				return this._alertWhispers;
			}
			set
			{
				this.SetProperty<bool>(ref this._alertWhispers, value, "AlertWhispers");
			}
		}
		public bool LogWhispers
		{
			get
			{
				return this._logWhispers;
			}
			set
			{
				this.SetProperty<bool>(ref this._logWhispers, value, "LogWhispers");
			}
		}
		public FontFamily FontFamily
		{
			get
			{
				return this._fontFamily;
			}
			set
			{
				this.SetProperty<FontFamily>(ref this._fontFamily, value, "FontFamily");
			}
		}
		public int FontSize
		{
			get
			{
				return this._fontSize;
			}
			set
			{
				this.SetProperty<int>(ref this._fontSize, value, "FontSize");
			}
		}

		public ChatSettingsWindowViewModel(GameSettings gameSettings)
		{
			this.GameSettings = gameSettings;
			this.FilterModoChat = this.GameSettings.FilterModoChat;
			this.AlertModoChat = this.GameSettings.AlertModoChat;
			this.LogModoChat = this.GameSettings.LogModoChat;
			this.FilterArbChat = this.GameSettings.FilterArbChat;
			this.AlertArbChat = this.GameSettings.AlertArbChat;
			this.LogArbChat = this.GameSettings.LogArbChat;
			this.FilterServeurMessages = this.GameSettings.FilterServeurMessages;
			this.AlertServeurMessages = this.GameSettings.AlertServeurMessages;
			this.LogServeurMessages = this.GameSettings.LogServeurMessages;
			this.FilterMapCrewChat = this.GameSettings.FilterMapCrewChat;
			this.AlertMapCrewChat = this.GameSettings.AlertMapCrewChat;
			this.LogMapCrewChat = this.GameSettings.LogMapCrewChat;
			this.FilterLuaTeamChat = this.GameSettings.FilterLuaTeamChat;
			this.AlertLuaTeamChat = this.GameSettings.AlertLuaTeamChat;
			this.LogLuaTeamChat = this.GameSettings.LogLuaTeamChat;
			this.FilterFunCorpChat = this.GameSettings.FilterFunCorpChat;
			this.AlertFunCorpChat = this.GameSettings.AlertFunCorpChat;
			this.LogFunCorpChat = this.GameSettings.LogFunCorpChat;
			this.FilterFashionSquadChat = this.GameSettings.FilterFashionSquadChat;
			this.AlertFashionSquadChat = this.GameSettings.AlertFashionSquadChat;
			this.LogFashionSquadChat = this.GameSettings.LogFashionSquadChat;
			this.FilterTribeChat = this.GameSettings.FilterTribeChat;
			this.AlertTribeChat = this.GameSettings.AlertTribeChat;
			this.LogTribeChat = this.GameSettings.LogTribeChat;
			this.FilterWhispers = this.GameSettings.FilterWhispers;
			this.AlertWhispers = this.GameSettings.AlertWhispers;
			this.LogWhispers = this.GameSettings.LogWhispers;
			this.FontFamily = this.GameSettings.FontFamily;
			this.FontSize = this.GameSettings.FontSize;
			this.FontSizes = new ObservableCollection<int> {22,21,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6};
			this.SaveCommand = new DelegateCommand(new Action(this.Save));
		}
		private void Save()
		{
			this.GameSettings.FilterModoChat = this.FilterModoChat;
			this.GameSettings.AlertModoChat = this.AlertModoChat;
			this.GameSettings.LogModoChat = this.LogModoChat;
			this.GameSettings.FilterArbChat = this.FilterArbChat;
			this.GameSettings.AlertArbChat = this.AlertArbChat;
			this.GameSettings.LogArbChat = this.LogArbChat;
			this.GameSettings.FilterServeurMessages = this.FilterServeurMessages;
			this.GameSettings.AlertServeurMessages = this.AlertServeurMessages;
			this.GameSettings.LogServeurMessages = this.LogServeurMessages;
			this.GameSettings.FilterMapCrewChat = this.FilterMapCrewChat;
			this.GameSettings.AlertMapCrewChat = this.AlertMapCrewChat;
			this.GameSettings.LogMapCrewChat = this.LogMapCrewChat;
			this.GameSettings.FilterLuaTeamChat = this.FilterLuaTeamChat;
			this.GameSettings.AlertLuaTeamChat = this.AlertLuaTeamChat;
			this.GameSettings.LogLuaTeamChat = this.LogLuaTeamChat;
			this.GameSettings.FilterFunCorpChat = this.FilterFunCorpChat;
			this.GameSettings.AlertFunCorpChat = this.AlertFunCorpChat;
			this.GameSettings.LogFunCorpChat = this.LogFunCorpChat;
			this.GameSettings.FilterFashionSquadChat = this.FilterFashionSquadChat;
			this.GameSettings.AlertFashionSquadChat = this.AlertFashionSquadChat;
			this.GameSettings.LogFashionSquadChat = this.LogFashionSquadChat;
			this.GameSettings.FilterTribeChat = this.FilterTribeChat;
			this.GameSettings.AlertTribeChat = this.AlertTribeChat;
			this.GameSettings.LogTribeChat = this.LogTribeChat;
			this.GameSettings.FilterWhispers = this.FilterWhispers;
			this.GameSettings.AlertWhispers = this.AlertWhispers;
			this.GameSettings.LogWhispers = this.LogWhispers;
			this.GameSettings.FontFamily = this.FontFamily;
			this.GameSettings.FontSize = this.FontSize;
			this.GameSettings.Save();
			EventHandler closed = this.Closed;
			if (closed == null)
			{
				return;
			}
			closed(this, new EventArgs());
		}
	}
}
