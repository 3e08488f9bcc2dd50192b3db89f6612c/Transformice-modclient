using System;

namespace tfmClientHook.Messages
{
	public abstract class Message
	{
		public abstract ByteBuffer GetBuffer();
	}
}
