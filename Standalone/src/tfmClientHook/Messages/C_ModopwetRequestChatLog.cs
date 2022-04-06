using System;
/**
Opens Chat Log from modopwet by username.
ByteBuffer -> [25, 27] (C - 25, CC - 27)
*/

namespace tfmClientHook.Messages
{
	public class C_ModopwetRequestChatLog : C_Message
	{
		public string Name { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(25);
			byteBuffer.WriteByte(27);
			byteBuffer.WriteString(this.Name);
			return byteBuffer;
		}
		public override bool IsEncrypted{ get{ return false; } }
	}
}
