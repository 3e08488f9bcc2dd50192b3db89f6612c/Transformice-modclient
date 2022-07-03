using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Transformice.tfmStandalone
{
	public class NewChatDialogWindow : PinnableWindow, IComponentConnector
	{
		internal TextBox TextBox;
		private bool _contentLoaded;
	
		public NewChatDialogWindow(NewChatViewModel viewModel)
		{
			this.InitializeComponent();
			base.DataContext = viewModel;
			viewModel.Accepted = (EventHandler)Delegate.Combine(viewModel.Accepted, new EventHandler(delegate(object sender, EventArgs args)
			{
				base.CanClose = true;
				base.DialogResult = new bool?(true);
				base.Close();
			}));
			viewModel.Cancelled = (EventHandler)Delegate.Combine(viewModel.Cancelled, new EventHandler(delegate(object sender, EventArgs args)
			{
				base.CanClose = true;
				base.DialogResult = new bool?(false);
				base.Close();
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
			Uri resourceLocator = new Uri("/Transformice;component/views/newchatdialogwindow.xaml", UriKind.Relative);
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
				this.TextBox = (TextBox)target;
				return;
			}
			this._contentLoaded = true;
		}
	}
}
