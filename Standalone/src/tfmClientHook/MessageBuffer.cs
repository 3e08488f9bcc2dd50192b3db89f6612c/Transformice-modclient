using System;

namespace tfmClientHook
{
	internal class MessageBuffer
	{
		private readonly bool _isClientBuffer;
		private ByteBuffer ByteBuffer { get; }
		public MessageBuffer(bool isClientBuffer)
		{
			this._isClientBuffer = isClientBuffer;
			this.ByteBuffer = new ByteBuffer();
		}
		public void AddBytes(byte[] byteList, int count)
		{
			this.ByteBuffer.WriteBytes(byteList);
		}
		public ByteBuffer GetNextMessage(ref byte fingerPrint)
		{
			if (this.ByteBuffer.Count == 0)
			{
				return null;
			}
			byte b = this.ByteBuffer.PeekByte(0);
			uint num;
			switch (b)
			{
				case 1:
					num = (uint)this.ByteBuffer.PeekByte(1);
					break;
				case 2:
					num = (uint)this.ByteBuffer.PeekUnsignedShort(1);
					break;
				case 3:
					num = this.ByteBuffer.PeekUnsignedMediumInt(1);
					break;
				case 4:
					num = this.ByteBuffer.PeekUnsignedInt(1);
					break;
				default:
					return null;
			}
			int num2 = (int)(1 + b + (this._isClientBuffer ? 1 : 0));
			if ((long)this.ByteBuffer.Count < (long)num2 + (long)((ulong)num))
			{
				return null;
			}
			if (this._isClientBuffer)
			{
				fingerPrint = this.ByteBuffer[num2 - 1];
			}
			this.ByteBuffer.ReadBytes(num2);
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteBytes(this.ByteBuffer.ReadBytes(num));
			return byteBuffer;
		}
	}
}
