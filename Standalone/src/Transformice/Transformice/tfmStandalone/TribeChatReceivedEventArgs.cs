using System;

namespace Transformice.tfmStandalone
{
	public sealed class TribeChatReceivedEventArgs : EventArgs
	{
		public string Message { get; set; }
	}
}
