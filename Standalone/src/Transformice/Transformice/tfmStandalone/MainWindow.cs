using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Transformice.tfmClientHook;
using Transformice.tfmClientHook.Messages;
using Transformice.tfmStandalone.Views;

namespace Transformice.tfmStandalone
{
	public class MainWindow : Window, IComponentConnector
	{
		public Rect GamePosition { get; private set; }
		private ClientHook ClientHook { get; }
		private MessageInterceptor MessageInterceptor { get; }
		private GameInfo GameInfo { get; }
		private GameSettings GameSettings { get; }
		private MenuOverlayWindow MenuOverlayWindow { get; }
		private ScreenShotOverlayWindow ScreenShotOverlayWindow { get; }
		private MapOverlayWindow MapOverlayWindow { get; }
		private CommunityOverlayWindow CommunityOverlayWindow { get; }
		private bool WasPreviousMaximized { get; set; }
		private bool IsFullScreen { get; set; }
		private DispatcherTimer ResizeTimer { get; }
		internal StackPanel GifRecordingStackPanel;
		internal FlashPlayer FlashPlayer;
		private bool _contentLoaded;
		
		[Dependency]
		public MainWindowViewModel MainWindowViewModel
		{
			get
			{
				return base.DataContext as MainWindowViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public MainWindow(ClientHook clientHook, MessageInterceptor messageInterceptor, GameInfo gameInfo, GameSettings gameSettings, WindowService windowService)
		{
			this.InitializeComponent();
			this.ClientHook = clientHook;
			this.MessageInterceptor = messageInterceptor;
			this.GameInfo = gameInfo;
			this.GameSettings = gameSettings;
			GameSettings gameSettings2 = this.GameSettings;
			gameSettings2.SettingsSaved = (EventHandler)Delegate.Combine(gameSettings2.SettingsSaved, new EventHandler(this.SettingsSaved));
			this.ResizeTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100.0)
			};
			this.ResizeTimer.Tick += this.ResizeTimerTick;
			CommunityOverlayViewModel communityOverlayViewModel = new CommunityOverlayViewModel(this.GameInfo, this.ClientHook);
			this.CommunityOverlayWindow = new CommunityOverlayWindow(communityOverlayViewModel);
			this.CommunityOverlayWindow.SizeChanged += delegate(object sender, SizeChangedEventArgs args)
			{
				this.UpdateOverlayPosition();
			};
			MessageInterceptor messageInterceptor2 = this.MessageInterceptor;
			messageInterceptor2.OSInformationSent = (EventHandler)Delegate.Combine(messageInterceptor2.OSInformationSent, new EventHandler(delegate(object sender, EventArgs args)
			{
				this.FlashPlayer.AnalyzeSWF();
			}));
			MessageInterceptor messageInterceptor3 = this.MessageInterceptor;
			messageInterceptor3.LoggedIn = (EventHandler)Delegate.Combine(messageInterceptor3.LoggedIn, (EventHandler)delegate
			{
				TaskHelpers.UiInvoke(delegate
				{
					communityOverlayViewModel.Initialize();
					CommunityOverlayWindow.Show();
					if (MainWindowViewModel.HasMaps)
					{
						MapOverlayWindow.Show();
					}
				});
			});
			FlashPlayer flashPlayer = this.FlashPlayer;
			flashPlayer.EncryptionKeyReceived = (EventHandler<sbyte[]>)Delegate.Combine(flashPlayer.EncryptionKeyReceived, new EventHandler<sbyte[]>(delegate(object sender, sbyte[] encryptionKey)
			{
				this.ClientHook.SetEncryptionKey(encryptionKey);
			}));
			FlashPlayer flashPlayer2 = this.FlashPlayer;
			flashPlayer2.EncryptionVectorReceived = (EventHandler<int[]>)Delegate.Combine(flashPlayer2.EncryptionVectorReceived, new EventHandler<int[]>(delegate(object sender, int[] encryptionVector)
			{
				this.ClientHook.SetEncryptionVector(encryptionVector);
			}));
			this.FlashPlayer.WindowService = windowService;
			this.FlashPlayer.GameSettings = gameSettings;
			this.MenuOverlayWindow = new MenuOverlayWindow(new MenuOverlayViewModel(this.FlashPlayer, this.GameSettings, windowService));
			this.MenuOverlayWindow.SizeChanged += delegate(object sender, SizeChangedEventArgs args)
			{
				this.UpdateOverlayPosition();
			};
			this.ScreenShotOverlayWindow = new ScreenShotOverlayWindow(this.GameSettings);
			ScreenShotOverlayWindow screenShotOverlayWindow = this.ScreenShotOverlayWindow;
			screenShotOverlayWindow.GifStartedRecording = (EventHandler)Delegate.Combine(screenShotOverlayWindow.GifStartedRecording, new EventHandler(delegate(object sender, EventArgs args)
			{
				this.GifRecordingStackPanel.Visibility = Visibility.Visible;
			}));
			ScreenShotOverlayWindow screenShotOverlayWindow2 = this.ScreenShotOverlayWindow;
			screenShotOverlayWindow2.GifFinishedRecording = (EventHandler)Delegate.Combine(screenShotOverlayWindow2.GifFinishedRecording, new EventHandler(delegate(object sender, EventArgs args)
			{
				this.GifRecordingStackPanel.Visibility = Visibility.Collapsed;
			}));
			this.MapOverlayWindow = new MapOverlayWindow(new MapOverlayViewModel(this.ClientHook));
			this.MapOverlayWindow.SizeChanged += delegate(object sender, SizeChangedEventArgs args)
			{
				this.UpdateOverlayPosition();
			};
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate()
			{
				this.ClientHook.Run(this.MessageInterceptor);
				this.MenuOverlayWindow.Owner = this;
				this.ScreenShotOverlayWindow.Owner = this;
				this.MapOverlayWindow.Owner = this;
				this.CommunityOverlayWindow.Owner = this;
				this.MenuOverlayWindow.Show();
				this.UpdateOverlayPosition();
			}));
		}

		private void SettingsSaved(object o, EventArgs eventArgs)
		{
			this.UpdateOverlayPosition();
		}

		private void HandleWindowLoaded(object sender, RoutedEventArgs e)
		{
			Random random = new Random();
			PropertyKey propertyKey = new PropertyKey(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);
			WindowProperties.SetWindowProperty(this, propertyKey, "tfmMainWindow" + random.Next(1, int.MaxValue));
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key == Key.F3)
			{
				this.ClientHook.SendToServer(ConnectionType.Main, new C_ChangeRoom
				{
					RoomName = this.GameSettings.HomeRoom
				});
				return;
			}
			if (key == Key.F5)
			{
				this.ClientHook.Stop();
				this.GameInfo.Clear();
				this.MainWindowViewModel.Username = null;
				this.FlashPlayer.Reload();
				this.ClientHook.Run(this.MessageInterceptor);
				return;
			}
			if (key != Key.F11)
			{
				return;
			}
			this.IsFullScreen = !this.IsFullScreen;
			if (!this.IsFullScreen)
			{
				base.WindowStyle = WindowStyle.SingleBorderWindow;
				base.WindowState = WindowState.Normal;
				if (this.WasPreviousMaximized)
				{
					base.WindowState = WindowState.Maximized;
					return;
				}
			}
			else
			{
				if (base.WindowState == WindowState.Maximized)
				{
					this.WasPreviousMaximized = true;
					base.WindowState = WindowState.Normal;
				}
				base.WindowStyle = WindowStyle.None;
				base.WindowState = WindowState.Maximized;
			}
		}

		private void HandleWindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateOverlayPosition();
		}

		private void HandleWindowLocationChanged(object sender, EventArgs e)
		{
			this.UpdateOverlayPosition();
		}

		private void UpdateOverlayPosition()
		{
			this.ResizeTimer.Stop();
			this.ResizeTimer.Start();
			Point point = this.FlashPlayer.PointToScreen(new Point(0.0, 0.0));
			this.MenuOverlayWindow.Left = point.X;
			this.MenuOverlayWindow.Top = point.Y;
			this.ScreenShotOverlayWindow.Left = point.X;
			this.ScreenShotOverlayWindow.Top = point.Y;
			this.ScreenShotOverlayWindow.Width = this.FlashPlayer.ActualWidth;
			this.ScreenShotOverlayWindow.Height = this.FlashPlayer.ActualHeight;
			this.MapOverlayWindow.Left = point.X + this.FlashPlayer.ActualWidth - this.MapOverlayWindow.ActualWidth;
			this.MapOverlayWindow.Top = point.Y;
			this.MapOverlayWindow.Height = this.FlashPlayer.ActualHeight;
			this.CommunityOverlayWindow.Left = point.X;
			this.CommunityOverlayWindow.Top = point.Y + this.FlashPlayer.ActualHeight - this.CommunityOverlayWindow.ActualHeight;
		}

		private void ResizeTimerTick(object sender, EventArgs e)
		{
			this.ResizeTimer.Stop();
			Point point = this.FlashPlayer.PointToScreen(new Point(0.0, 0.0));
			double num = point.X;
			double num2 = point.Y;
			double num3 = this.FlashPlayer.ActualWidth;
			double num4 = this.FlashPlayer.ActualHeight;
			if (this.GameSettings.ZoomMode != FlashPlayer.ZoomModeEnum.Stretch)
			{
				if (this.GameSettings.ZoomMode == FlashPlayer.ZoomModeEnum.NoZoom)
				{
					num3 = 800.0;
					num4 = 600.0;
				}
				else if (this.FlashPlayer.ActualWidth / this.FlashPlayer.ActualHeight > 1.3333333333333333)
				{
					num4 = this.FlashPlayer.ActualHeight;
					num3 = 1.3333333333333333 * num4;
				}
				else
				{
					num3 = this.FlashPlayer.ActualWidth;
					num4 = 0.75 * num3;
				}
				switch (this.GameSettings.AlignmentMode)
				{
					case FlashPlayer.AlignmentModeEnum.Center:
						num = point.X + (this.FlashPlayer.ActualWidth - num3) / 2.0;
						num2 = point.Y + (this.FlashPlayer.ActualHeight - num4) / 2.0;
						break;
					case FlashPlayer.AlignmentModeEnum.CenterLeft:
						num = point.X;
						num2 = point.Y + (this.FlashPlayer.ActualHeight - num4) / 2.0;
						break;
					case FlashPlayer.AlignmentModeEnum.CenterRight:
						num = point.X + this.FlashPlayer.ActualWidth - num3;
						num2 = point.Y + (this.FlashPlayer.ActualHeight - num4) / 2.0;
						break;
					case FlashPlayer.AlignmentModeEnum.TopCenter:
						num = point.X + (this.FlashPlayer.ActualWidth - num3) / 2.0;
						num2 = point.Y;
						break;
					case FlashPlayer.AlignmentModeEnum.TopLeft:
						num = point.X;
						num2 = point.Y;
						break;
					case FlashPlayer.AlignmentModeEnum.TopRight:
						num = point.X + this.FlashPlayer.ActualWidth - num3;
						num2 = point.Y;
						break;
					case FlashPlayer.AlignmentModeEnum.BottomCenter:
						num = point.X + (this.FlashPlayer.ActualWidth - num3) / 2.0;
						num2 = point.Y + this.FlashPlayer.ActualHeight - num4;
						break;
					case FlashPlayer.AlignmentModeEnum.BottomLeft:
						num = point.X;
						num2 = point.Y + this.FlashPlayer.ActualHeight - num4;
						break;
					case FlashPlayer.AlignmentModeEnum.BottomRight:
						num = point.X + this.FlashPlayer.ActualWidth - num3;
						num2 = point.Y + this.FlashPlayer.ActualHeight - num4;
						break;
				}
			}
			if (num < point.X)
			{
				num = point.X;
			}
			if (num2 < point.Y)
			{
				num2 = point.Y;
			}
			if (num3 > this.FlashPlayer.ActualWidth)
			{
				num3 = this.FlashPlayer.ActualWidth;
			}
			if (num4 > this.FlashPlayer.ActualHeight)
			{
				num4 = this.FlashPlayer.ActualHeight;
			}
			this.GamePosition = new Rect(num, num2, num3, num4);
		}

		private void HandleScreenshotClick(object sender, MouseButtonEventArgs e)
		{
			this.ScreenShotOverlayWindow.Show(false);
		}

		private void HandleGifClick(object sender, MouseButtonEventArgs e)
		{
			this.ScreenShotOverlayWindow.Show(true);
		}

		private void SaveGifRecordingClick(object sender, MouseButtonEventArgs e)
		{
			this.ScreenShotOverlayWindow.IsRecording = false;
		}

		private void StopGifRecordingClick(object sender, MouseButtonEventArgs e)
		{
			this.ScreenShotOverlayWindow.SaveRecording = false;
			this.ScreenShotOverlayWindow.IsRecording = false;
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			this.GameSettings.Save();
			this.ScreenShotOverlayWindow.Close();
			this.MapOverlayWindow.Close();
			this.ClientHook.Stop();
			Application.Current.Shutdown(0);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/Transformice;component/views/mainwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
				case 1:
					((MainWindow)target).Closing += this.OnClosing;
					((MainWindow)target).Loaded += this.HandleWindowLoaded;
					((MainWindow)target).SizeChanged += this.HandleWindowSizeChanged;
					((MainWindow)target).LocationChanged += this.HandleWindowLocationChanged;
					return;
				case 2:
					((Image)target).MouseLeftButtonUp += this.HandleScreenshotClick;
					return;
				case 3:
					((Image)target).MouseLeftButtonUp += this.HandleGifClick;
					return;
				case 4:
					this.GifRecordingStackPanel = (StackPanel)target;
					return;
				case 5:
					((Image)target).MouseUp += this.SaveGifRecordingClick;
					return;
				case 6:
					((Image)target).MouseUp += this.StopGifRecordingClick;
					return;
				case 7:
					this.FlashPlayer = (FlashPlayer)target;
					return;
				default:
					this._contentLoaded = true;
					return;
			}
		}
	}
}
