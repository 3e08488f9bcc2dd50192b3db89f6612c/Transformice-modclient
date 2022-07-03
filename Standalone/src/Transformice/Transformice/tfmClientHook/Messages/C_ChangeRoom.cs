using System;

namespace Transformice.tfmClientHook.Messages
{
	public class C_ChangeRoom : C_Message
	{
		public string RoomName { get; set; }
		public override bool IsEncrypted
		{
			get
			{
				return false;
			}
		}
		
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
	}
}
