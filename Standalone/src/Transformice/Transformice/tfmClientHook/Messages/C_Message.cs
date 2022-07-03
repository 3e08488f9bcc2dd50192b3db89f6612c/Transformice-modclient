using System;

namespace Transformice.tfmClientHook.Messages
{
	public abstract class C_Message : Message
	{
		public abstract bool IsEncrypted { get; }
	}
}
