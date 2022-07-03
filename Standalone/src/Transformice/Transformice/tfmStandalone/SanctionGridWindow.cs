using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Transformice.tfmStandalone
{
	public class SanctionGridWindow : Window, IComponentConnector
	{

		internal HeaderScrollViewer HeaderScrollViewer;
		internal ScrollViewer ContentScrollViewer;
		internal ScrollViewer RowHeaderScrollViewer;
		internal ItemsControl RowHeaderItems;
		private bool _contentLoaded;
		
		public SanctionGridWindow(SanctionGridViewModel viewModel)
		{
			this.InitializeComponent();
			this.ViewModel = viewModel;
		}
		
		public	SanctionGridViewModel ViewModel
		{
			get
			{
				return base.DataContext as SanctionGridViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			this.HeaderScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
			this.RowHeaderScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
		}

		private void RowHeaderScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (this.ContentScrollViewer.VerticalOffset != e.VerticalOffset)
			{
				this.ContentScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
			}
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
			Uri resourceLocator = new Uri("/Transformice;component/views/sanctiongridwindow.xaml", UriKind.Relative);
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
				this.HeaderScrollViewer = (HeaderScrollViewer)target;
				return;
			case 2:
				this.ContentScrollViewer = (ScrollViewer)target;
				this.ContentScrollViewer.ScrollChanged += this.ScrollViewer_OnScrollChanged;
				return;
			case 3:
				this.RowHeaderScrollViewer = (ScrollViewer)target;
				this.RowHeaderScrollViewer.ScrollChanged += this.RowHeaderScrollViewer_OnScrollChanged;
				return;
			case 4:
				this.RowHeaderItems = (ItemsControl)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
