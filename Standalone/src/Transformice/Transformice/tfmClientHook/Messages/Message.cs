using System;

namespace Transformice.tfmClientHook.Messages
{
	public abstract class Message
	{
		public abstract ByteBuffer GetBuffer();
	}
}
