using System;
using System.Windows;
using System.Windows.Controls;

namespace Transformice.tfmStandalone
{
	public class HeaderScrollViewer : ScrollViewer
	{
		public object Header
		{
			get
			{
				return base.GetValue(HeaderScrollViewer.HeaderProperty);
			}
			set
			{
				base.SetValue(HeaderScrollViewer.HeaderProperty, value);
			}
		}

		public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HeaderScrollViewer), new FrameworkPropertyMetadata(null));
	}
}
