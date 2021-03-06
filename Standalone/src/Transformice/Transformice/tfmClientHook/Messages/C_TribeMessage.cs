using System;

namespace Transformice.tfmClientHook.Messages
{
	public class C_TribeMessage : C_TribulleMessage
	{
		public int SequenceId { get; set; }
		public string Message { get; set; }
		
		public override short TribulleId
		{
			get
			{
				return ServerInfo.OutgoingTribulleCodes.TribeMessage;
			}
		}
		
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteByte(60);
			byteBuffer.WriteByte(3);
			byteBuffer.WriteShort(this.TribulleId);
			byteBuffer.WriteInt(this.SequenceId);
			byteBuffer.WriteString(this.Message);
			return byteBuffer;
		}
	}
}
