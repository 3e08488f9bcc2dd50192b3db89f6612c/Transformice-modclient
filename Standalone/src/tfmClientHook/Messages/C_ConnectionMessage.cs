using System;
/**
When player log in.
ByteBuffer -> [28, 1] (C - 28, CC - 1)
*/

namespace tfmClientHook.Messages
{
	// Token: 0x020000C2 RID: 194
	public class C_ConnectionMessage : C_Message
	{
		public short Version { get; set; } // swf version
		public string Key { get; set; } // swf key
		public string PlayerType { get; set; }
		public string Browser { get; set; }
		public int LoaderSize { get; set; }
		public string Fonts { get; set; }
		public string ServerString { get; set; }
		public int ReferrerId { get; set; }
		public int CurrentTime { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(28);
			byteBuffer.WriteByte(1);
			byteBuffer.WriteShort(this.Version);
			byteBuffer.WriteString(this.Key);
			byteBuffer.WriteString(this.PlayerType);
			byteBuffer.WriteString(this.Browser);
			byteBuffer.WriteInt(this.LoaderSize);
			byteBuffer.WriteString("");
			byteBuffer.WriteString(this.Fonts);
			byteBuffer.WriteString(this.ServerString);
			byteBuffer.WriteInt(this.ReferrerId);
			byteBuffer.WriteInt(this.CurrentTime);
			byteBuffer.WriteString("");
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return false; } }
	}
}
