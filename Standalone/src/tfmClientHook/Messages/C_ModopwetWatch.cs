using System;
/**
Watchs player using modopwet.
ByteBuffer -> [25, 24] (C - 25, CC - 24)
*/

namespace tfmClientHook.Messages
{
	public class C_ModopwetWatch : C_Message
	{
		public string Name { get; set; }
		public bool IsFollowing { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(25);
			byteBuffer.WriteByte(24);
			byteBuffer.WriteString(this.Name);
			byteBuffer.WriteBool(this.IsFollowing);
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return false; } }
	}
}
