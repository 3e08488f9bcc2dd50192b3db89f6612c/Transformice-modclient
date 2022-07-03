using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Prism.Commands;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public class MainWindowViewModel : BindableBase
	{
		public DelegateCommand ToggleChatCommand { get; }
		public DelegateCommand ToggleModopwetCommand { get; }
		public DelegateCommand ShowVpnFarmingWindowCommand { get; }
		public DelegateCommand ShowVpnFarmingRoomWindowCommand { get; }
		public DelegateCommand UpdateCommand { get; }
		private MessageInterceptor MessageInterceptor { get; }
		private GameInfo GameInfo { get; }
		private WindowService WindowService { get; }
		private UpdateService UpdateService { get; }
		private string _status;
		private string _username;
		private bool _isChatShowing;
		private bool _isModopwetShowing;
		private bool _hasMaps;
		private double _version;
		private bool _isNewVersionAvailable;
		private double _newVersion;
		
		public string Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this.SetProperty<string>(ref this._status, value, "Status");
			}
		}

		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this.SetProperty<string>(ref this._username, value, "Username");
			}
		}

		public bool IsChatShowing
		{
			get
			{
				return this._isChatShowing;
			}
			set
			{
				this.SetProperty<bool>(ref this._isChatShowing, value, "IsChatShowing");
			}
		}

		public bool IsModopwetShowing
		{
			get
			{
				return this._isModopwetShowing;
			}
			set
			{
				this.SetProperty<bool>(ref this._isModopwetShowing, value, "IsModopwetShowing");
			}
		}

		public bool HasMaps
		{
			get
			{
				return this._hasMaps;
			}
			set
			{
				this.SetProperty<bool>(ref this._hasMaps, value, "HasMaps");
			}
		}

		public double Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this.SetProperty<double>(ref this._version, value, "Version");
			}
		}

		public bool IsNewVersionAvailable
		{
			get
			{
				return this._isNewVersionAvailable;
			}
			set
			{
				this.SetProperty<bool>(ref this._isNewVersionAvailable, value, "IsNewVersionAvailable");
			}
		}

		public double NewVersion
		{
			get
			{
				return this._newVersion;
			}
			set
			{
				this.SetProperty<double>(ref this._newVersion, value, "NewVersion");
			}
		}

		public MainWindowViewModel(MessageInterceptor messageInterceptor, GameInfo gameInfo, WindowService windowService, UpdateService updateService)
		{
			this.MessageInterceptor = messageInterceptor;
			this.GameInfo = gameInfo;
			this.WindowService = windowService;
			this.UpdateService = updateService;
			MessageInterceptor messageInterceptor2 = this.MessageInterceptor;
			messageInterceptor2.LoggedIn = (EventHandler)Delegate.Combine(messageInterceptor2.LoggedIn, new EventHandler(delegate(object sender, EventArgs args)
			{
				this.Username = this.GameInfo.Name;
			}));
			UpdateService updateService2 = this.UpdateService;
			updateService2.LatestVersionReceived = (EventHandler<double>)Delegate.Combine(updateService2.LatestVersionReceived, new EventHandler<double>(delegate(object sender, double newVersion)
			{
				this.NewVersion = newVersion;
				this.IsNewVersionAvailable = (this.NewVersion > this.Version);
			}));
			this.UpdateService.Initialize();
			this.ToggleChatCommand = new DelegateCommand(delegate()
			{
				this.IsChatShowing = !this.IsChatShowing;
				this.GameInfo.IsChatShowing = this.IsChatShowing;
				if (this.IsChatShowing)
				{
					this.WindowService.ShowChatWindow();
					return;
				}
				this.WindowService.HideChatWindow();
			});
			this.ToggleModopwetCommand = new DelegateCommand(delegate()
			{
				this.IsModopwetShowing = !this.IsModopwetShowing;
				this.GameInfo.IsModopwetShowing = this.IsModopwetShowing;
				if (this.IsModopwetShowing)
				{
					this.WindowService.ShowModopwetWindow();
					return;
				}
				this.WindowService.HideModopwetWindow();
			});
			this.ShowVpnFarmingWindowCommand = new DelegateCommand(delegate()
			{
				this.WindowService.ShowVpnFarmingWindow();
			});
			this.ShowVpnFarmingRoomWindowCommand = new DelegateCommand(delegate()
			{
				this.WindowService.ShowVpnFarmingRoomWindow();
			});
			this.UpdateCommand = new DelegateCommand(delegate()
			{
				this.WindowService.ShowUpdateConfirmationDialog(this.UpdateService);
			});
			this.IsChatShowing = true;
			if (!Directory.Exists(MapOverlayViewModel.MapsFolder))
			{
				this.HasMaps = false;
			}
			else
			{
				int num = Directory.EnumerateFiles(MapOverlayViewModel.MapsFolder, "*.png", SearchOption.AllDirectories).Count<string>();
				this.HasMaps = (num > 0);
			}
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			double version2;
			if (double.TryParse(string.Format("{0}.{1}", version.Major, version.Minor), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out version2))
			{
				this.Version = version2;
			}
		}
	}
}
