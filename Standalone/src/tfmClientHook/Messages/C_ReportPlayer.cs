using System;
/**
Reports a player.
ByteBuffer -> [8, 25] (C - 8, CC - 25)
*/

namespace tfmClientHook.Messages
{
	public class C_ReportPlayer : C_Message
	{
		public string Name { get; set; }
		public ReportType Type { get; set; } // Hack, Phishing, Spam / Flood, etc.
		public string Comment { get; set; } // report comment
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(8);
			byteBuffer.WriteByte(25);
			byteBuffer.WriteString(this.Name);
			byteBuffer.WriteByte((byte)this.Type);
			byteBuffer.WriteByte(0);
			if (string.IsNullOrEmpty(this.Comment)) byteBuffer.WriteByte(0);
			else byteBuffer.WriteString(this.Comment);
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return false; } }
	}
}
