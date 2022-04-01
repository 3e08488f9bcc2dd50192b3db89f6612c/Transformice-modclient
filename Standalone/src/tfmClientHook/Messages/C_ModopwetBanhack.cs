using System;
/**
Execute /banhack command in modopwet
ByteBuffer -> [25, 25] (C - 25, CC - 25)
*/

namespace tfmClientHook.Messages
{
	public class C_ModopwetBanhack : C_Message
	{
		public string Name { get; set; }
		public bool IsInvisible { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(25);
			byteBuffer.WriteByte(25);
			byteBuffer.WriteString(this.Name);
			byteBuffer.WriteBool(this.IsInvisible);
			return byteBuffer;
		}
		public override bool IsEncrypted { get{ return false; } }
	}
}
