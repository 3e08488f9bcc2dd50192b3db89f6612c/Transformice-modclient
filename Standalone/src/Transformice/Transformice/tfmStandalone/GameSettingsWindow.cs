using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Transformice.tfmStandalone
{
	public class GameSettingsWindow : PinnableWindow, IComponentConnector
	{
		internal GameSettingsWindow Window;
		private bool _contentLoaded;
		
		public GameSettingsWindow(GameSettingsViewModel viewModel)
		{
			this.InitializeComponent();
			base.DataContext = viewModel;
			viewModel.Closed = (EventHandler)Delegate.Combine(viewModel.Closed, new EventHandler(delegate(object sender, EventArgs e)
			{
				base.Hide();
			}));
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
			Uri resourceLocator = new Uri("/Transformice;component/views/gamesettingswindow.xaml", UriKind.Relative);
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
			if (connectionId == 1)
			{
				this.Window = (GameSettingsWindow)target;
				return;
			}
			this._contentLoaded = true;
		}
	}
}
