using System;
/**
Client sends command.
ByteBuffer -> [6, 26] (C - 6, CC - 26)
*/

namespace tfmClientHook.Messages
{
	public class C_Command : C_Message
	{
		public string Command { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(6);
			byteBuffer.WriteByte(26);
			byteBuffer.WriteString(this.Command);
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return true; } }
	}
}
