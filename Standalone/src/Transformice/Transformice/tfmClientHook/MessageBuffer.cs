using System;

namespace Transformice.tfmClientHook
{
	internal class MessageBuffer
	{
		private ByteBuffer ByteBuffer { get; }
		private readonly bool _isClientBuffer;
		
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
			if(this.ByteBuffer.Count == 0)
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
			int num2 = 1 + b + (_isClientBuffer ? 1 : 0);
			if (ByteBuffer.Count < num2 + num)
			{
				return null;
			}
			if (_isClientBuffer)
			{
				fingerPrint = ByteBuffer[num2 - 1];
			}
			ByteBuffer.ReadBytes(num2);
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteBytes(ByteBuffer.ReadBytes(num));
			return byteBuffer;
		}
	}
}
