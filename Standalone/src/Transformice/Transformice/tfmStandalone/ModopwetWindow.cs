using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace Transformice.tfmStandalone
{
	public class ModopwetWindow : PinnableWindow, IComponentConnector
	{
		public bool IsApplicationClosing { get; set; }
		internal ModopwetWindow Window;
		internal Button CommunitySelectionDropDownButton;
		internal Button SettingsPopupButton;
		internal Button AutorefreshButton;
		private bool _contentLoaded;
		
		private ModopwetViewModel ModopwetViewModel
		{
			get
			{
				return (ModopwetViewModel)base.DataContext;
			}
			set
			{
				base.DataContext = value;
			}
		}
		
		public ModopwetWindow(ModopwetViewModel viewModel)
		{
			this.InitializeComponent();
			this.ModopwetViewModel = viewModel;
			ModopwetViewModel modopwetViewModel = this.ModopwetViewModel;
			modopwetViewModel.NewReportReceived = (EventHandler)Delegate.Combine(modopwetViewModel.NewReportReceived, new EventHandler(delegate(object o, EventArgs args)
			{
				if ((!base.IsActive && !base.Topmost) || base.WindowState == WindowState.Minimized)
				{
					WindowFlash.FlashWindow(this);
				}
			}));
		}

		private void HandleActivated(object sender, EventArgs e)
		{
			WindowFlash.StopFlashing(this);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = !this.IsApplicationClosing;
			base.Hide();
			this.ModopwetViewModel.WindowHidden();
		}

		private void HandleLoaded(object sender, RoutedEventArgs e)
		{
			Random random = new Random();
			PropertyKey propertyKey = new PropertyKey(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);
			WindowProperties.SetWindowProperty(this, propertyKey, "tfmModopwetWindow" + random.Next(1, int.MaxValue));
		}

		private void HandleIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.ModopwetViewModel.WindowVisibilityChanged(base.IsVisible);
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/Transformice;component/views/modopwetwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
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
					this.Window = (ModopwetWindow)target;
					return;
				case 2:
					this.CommunitySelectionDropDownButton = (Button)target;
					return;
				case 3:
					this.SettingsPopupButton = (Button)target;
					return;
				case 4:
					this.AutorefreshButton = (Button)target;
					return;
				default:
					this._contentLoaded = true;
					return;
			}
		}
	}
}
