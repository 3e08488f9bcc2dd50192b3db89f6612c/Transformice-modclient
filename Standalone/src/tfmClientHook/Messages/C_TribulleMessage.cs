using System;

namespace tfmClientHook.Messages
{
	public abstract class C_TribulleMessage : C_Message
	{
		public override bool IsEncrypted { get { return true; } }
		public override ByteBuffer GetBuffer() { return null; }
		public abstract short TribulleId { get; }
	}
}
