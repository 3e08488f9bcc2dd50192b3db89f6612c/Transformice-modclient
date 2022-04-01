using System;
using System.Collections.Generic;
/**
Subscribes to modopwet notifications
ByteBuffer -> [25, 12] (C - 25, CC - 12)
*/

namespace tfmClientHook.Messages
{
	public class C_ModopwetSubscribeNotifications : C_Message
	{
		public bool IsSubscribed { get; set; }
		public List<string> Communities { get; set; }
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(25);
			byteBuffer.WriteByte(12);
			byteBuffer.WriteBool(this.IsSubscribed);
			byteBuffer.WriteByte((byte)this.Communities.Count);
			foreach (string s in this.Communities)
			{
				byteBuffer.WriteString(s);
			}
			return byteBuffer;
		}
		public override bool IsEncrypted { get { return false; } }
	}
}
