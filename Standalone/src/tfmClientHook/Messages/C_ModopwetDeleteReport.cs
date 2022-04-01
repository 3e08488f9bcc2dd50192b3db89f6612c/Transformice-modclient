using System;
/**
Deletes report from modopwet.
ByteBuffer -> [25, 23] (C - 25, CC - 23)
*/

namespace tfmClientHook.Messages
{
	public class C_ModopwetDeleteReport : C_Message
	{
		public string Name { get; set; }
		public bool IsHandled { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(25);
			byteBuffer.WriteByte(23);
			byteBuffer.WriteString(this.Name);
			byteBuffer.WriteBool(this.IsHandled);
			return byteBuffer;
		}
		public override bool IsEncrypted { get{ return false; } }
	}
}
