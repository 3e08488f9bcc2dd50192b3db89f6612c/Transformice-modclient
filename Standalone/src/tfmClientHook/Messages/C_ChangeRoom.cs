using System;
/**
Client changes the room.
ByteBuffer -> [5, 38] (C - 5, CC - 38)
*/

namespace tfmClientHook.Messages
{
	public class C_ChangeRoom : C_Message
	{
		public string RoomName { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(5);
			byteBuffer.WriteByte(38);
			byteBuffer.WriteByte(byte.MaxValue);
			byteBuffer.WriteString(this.RoomName);
			byteBuffer.WriteByte(0);
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return false; } }
	}
}
