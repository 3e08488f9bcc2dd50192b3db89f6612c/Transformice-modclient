using System;

namespace tfmClientHook.Messages
{
	public abstract class S_TribulleMessage : S_Message
	{
		public override ByteBuffer GetBuffer()
		{
			return null;
		}
		public abstract short TribulleId { get; }
	}
}
