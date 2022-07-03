using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Practices.Unity;

namespace Transformice.tfmStandalone
{
	public class VpnFarmingRoomWindow : PinnableWindow, IComponentConnector
	{
		internal VpnFarmingRoomWindow Window;
		internal Grid ResultsGrid;
		private bool _contentLoaded;
		
		[Dependency]
		public VpnFarmingRoomWindowViewModel ViewModel
		{
			get
			{
				return base.DataContext as VpnFarmingRoomWindowViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public VpnFarmingRoomWindow()
		{
			this.InitializeComponent();
		}
		
		protected override void OnClosing(CancelEventArgs e)
		{
			this.ViewModel.Closing();
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
			Uri resourceLocator = new Uri("/Transformice;component/views/vpnfarmingroomwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.Window = (VpnFarmingRoomWindow)target;
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			this.ResultsGrid = (Grid)target;
		}
	}
}
