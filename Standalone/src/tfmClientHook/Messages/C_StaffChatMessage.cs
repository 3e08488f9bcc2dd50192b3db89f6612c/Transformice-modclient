using System;
/**
Send message in Staff Chats.
ByteBuffer -> [6, 10] (C - 6, CC - 10)
*/

namespace tfmClientHook.Messages
{
	public sealed class C_StaffChatMessage : C_Message
	{
		public StaffChatType Type { get; set; }
		public string Message { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(6);
			byteBuffer.WriteByte(10);
			byteBuffer.WriteByte((byte)this.Type);
			byteBuffer.WriteString(this.Message);
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return false; } }
	}
}
