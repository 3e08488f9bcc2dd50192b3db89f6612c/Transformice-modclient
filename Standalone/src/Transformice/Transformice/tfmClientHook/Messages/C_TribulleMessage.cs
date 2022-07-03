using System;

namespace Transformice.tfmClientHook.Messages
{
	public abstract class C_TribulleMessage : C_Message
	{
		public abstract short TribulleId { get; }
		
		public override bool IsEncrypted
		{
			get
			{
				return true;
			}
		}
		
		public override ByteBuffer GetBuffer()
		{
			return null;
		}
	}
}
