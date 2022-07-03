using System;
using System.Windows.Media;

namespace Transformice.tfmStandalone
{
	public sealed class RoomMemberViewModel
	{
		public string Name { get; set; }
		public string Ip { get; set; }
		public SolidColorBrush IpColor { get; set; }
		public string Country { get; set; }
	}
}
