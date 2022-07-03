using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Transformice.tfmClientHook.Messages;

namespace Transformice.tfmClientHook
{
	internal sealed class ProxyConnection
	{
		private const int MAX_BUFFER_SIZE = 2048;
		private static readonly byte[] PolicyFileRequest = new byte[] {60,112,111,108,105,99,121,45,102,105,108,101,45,114,101,113,117,101,115,116,47,62,0};
		public EventHandler ConnectionClosed;
		private readonly ConnectionType _connectionType;
		private readonly IMessageInterceptor _messageInterceptor;
		private bool _isGameServer;
		private bool _isRunning;
		private Socket _clientSocket;
		private byte[] _clientBuffer;
		private MessageBuffer _clientMessageBuffer;
		private int _clientMessageCount;
		private byte _clientFingerprint;
		private Socket _serverSocket;
		private byte[] _serverBuffer;
		private MessageBuffer _serverMessageBuffer;
		
		public ProxyConnection(ConnectionType connectionType, IMessageInterceptor messageInterceptor)
		{
			this._connectionType = connectionType;
			this._messageInterceptor = messageInterceptor;
		}

		public void Start(Socket clientSocket, IPAddress ipAddress, int port)
		{
			this._clientSocket = clientSocket;
			this._clientBuffer = new byte[MAX_BUFFER_SIZE];
			this._clientMessageBuffer = new MessageBuffer(isClientBuffer: true);
			this._clientMessageCount = 0;
			this._clientFingerprint = 0;
			this._serverSocket = null;
			this._serverBuffer = new byte[MAX_BUFFER_SIZE];
			this._serverMessageBuffer = new MessageBuffer(isClientBuffer: false);
			if (this._isRunning)
			{
				this.Stop();
			}
			this._isRunning = true;
			try
			{
				this._serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				this._serverSocket.BeginConnect(new IPEndPoint(ipAddress, port), delegate(IAsyncResult result)
				{
					if (result != null && result.IsCompleted && result.AsyncState is Socket)
					{
						Socket socket = (Socket)result.AsyncState;
						if (this._isRunning && socket != null)
						{
							socket.EndConnect(result);
							this._clientSocket.ReceiveBufferSize = 2048;
							socket.ReceiveBufferSize = 2048;
							this.ServerBeginReceive();
							this.ClientBeginReceive();
						}
					}
				}, this._serverSocket);
			}
			catch (ObjectDisposedException) {}
			catch (SocketException) {}
			catch (Exception ex3)
			{
				Console.WriteLine("Socket connection failed: " + ex3);
			}
		}

		public void Stop()
		{
			if (_isRunning)
			{
				this._clientSocket?.Close();
				this._clientSocket = null;
				this._serverSocket?.Close();
				this._serverSocket = null;
				this._isRunning = false;
				ConnectionClosed?.Invoke(this, new EventArgs());
			}
		}

		public void SendMessageToClient(S_Message message)
		{
			this.SendToClient(message.GetBuffer());
		}

		public void SendMessageToServer(C_Message message)
		{
			ByteBuffer buffer = message.GetBuffer();
			this.SendToServer(message.IsEncrypted ? this.EncryptMessage(buffer) : buffer);
		}

		private ByteBuffer EncryptMessage(ByteBuffer originalBuffer)
		{
			int num = (int)this._clientFingerprint;
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteBytes(originalBuffer.ReadBytes(originalBuffer.Count));
			originalBuffer.WriteShort(byteBuffer.ReadShort());
			int num2 = ServerInfo.EncryptionVector.Length;
			while (byteBuffer.Count > 0)
			{
				num = (num + 1) % num2;
				originalBuffer.WriteByte((byte)(byteBuffer.ReadByte() ^ ServerInfo.EncryptionVector[num]));
			}
			return originalBuffer;
		}

		private void ClientBeginReceive()
		{
			if (!this._isRunning)
			{
				return;
			}
			try
			{
				this._clientSocket.BeginReceive(this._clientBuffer, 0, MAX_BUFFER_SIZE, SocketFlags.None, this.OnClientReceiveData, this._clientSocket);
			}
			catch (SocketException)
			{
				this.Stop();
			}
			catch (Exception)
			{
				this.Stop();
			}
		}

		private void OnClientReceiveData(IAsyncResult result)
		{
			if (!this._isRunning || !result.IsCompleted || !(result.AsyncState is Socket))
			{
				this.Stop();
				return;
			}
			Socket socket = (Socket)result.AsyncState;
			int num;
			try
			{
				num = socket.EndReceive(result);
				if (num == 0)
				{
					Stop();
					return;
				}
			}
			catch
			{
				this.Stop();
				return;
			}
			byte[] array = new byte[num];
			Array.Copy(this._clientBuffer, 0, array, 0, num);
			if (!this._isGameServer && array.SequenceEqual(PolicyFileRequest))
			{
				Console.WriteLine("Connected to policy file request.");
				SendBytesToServer(array);
				return;
			}
			this._isGameServer = true;
			this._clientMessageBuffer.AddBytes(array, num);
			byte fingerPrint = 0;
			for (ByteBuffer nextMessage = this._clientMessageBuffer.GetNextMessage(ref fingerPrint); nextMessage != null; nextMessage = this._clientMessageBuffer.GetNextMessage(ref fingerPrint))
			{
				if (++this._clientMessageCount == 2)
				{
					this._clientFingerprint = fingerPrint;
				}
				try
				{
					bool sendToServer = true;
					C_Message c_Message = MessageProcessor.ClientProcessMessage(this._messageInterceptor, nextMessage, fingerPrint, ref sendToServer);
					if (sendToServer)
					{
						if (c_Message != null && c_Message.IsEncrypted)
						{
							this.SendToServer(EncryptMessage(c_Message.GetBuffer()));
						}
						else
						{
							this.SendToServer((c_Message == null) ? nextMessage : c_Message.GetBuffer());
						}
					}
				}
				catch (Exception)
				{
					this.SendToServer(nextMessage);
				}
			}
			this.ClientBeginReceive();
		}

		private void SendToClient(ByteBuffer bytesToSend)
		{
			if (!this._isRunning)
			{
				return;
			}
			if (bytesToSend.Count <= 255)
			{
				bytesToSend.PrependByte((byte)bytesToSend.Count);
				bytesToSend.PrependByte(1);
			}
			else if (bytesToSend.Count <= 65535)
			{
				bytesToSend.PrependShort((short)bytesToSend.Count);
				bytesToSend.PrependByte(2);
			}
			else
			{
				bytesToSend.PrependMediumInt(bytesToSend.Count);
				bytesToSend.PrependByte(3);
			}
			this.SendBytesToClient(bytesToSend.GetBytes());
		}

		private void SendBytesToClient(byte[] bytesToSend)
		{
			try
			{
				this._clientSocket.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None, delegate(IAsyncResult x)
				{
					if (!x.IsCompleted || !(x.AsyncState is Socket))
					{
						this.Stop();
						return;
					}
					try
					{
						((Socket)x.AsyncState).EndSend(x);
					}
					catch (Exception)
					{
						this.Stop();
					}
				}, this._clientSocket);
			}
			catch (Exception)
			{
				this.Stop();
			}
		}

		private void ServerBeginReceive()
		{
			if (!this._isRunning)
			{
				return;
			}
			try
			{
				this._serverSocket.BeginReceive(this._serverBuffer, 0, MAX_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(this.OnServerReceiveData), this._serverSocket);
			}
			catch (SocketException)
			{
				this.Stop();
			}
			catch (Exception)
			{
				this.Stop();
			}
		}

		private void OnServerReceiveData(IAsyncResult result)
		{
			if (!this._isRunning || !result.IsCompleted || !(result.AsyncState is Socket))
			{
				this.Stop();
				return;
			}
			Socket socket = (Socket)result.AsyncState;
			int num;
			try
			{
				num = socket.EndReceive(result);
				if (num == 0)
				{
					this.Stop();
					return;
				}
			}
			catch
			{
				this.Stop();
				return;
			}
			byte[] array = new byte[num];
			Array.Copy(this._serverBuffer, 0, array, 0, num);
			if (!this._isGameServer)
			{
				this.SendBytesToClient(array);
				this.Stop();
				return;
			}
			this._serverMessageBuffer.AddBytes(array, num);
			byte b = 0;
			for (ByteBuffer nextMessage = this._serverMessageBuffer.GetNextMessage(ref b); nextMessage != null; nextMessage = this._serverMessageBuffer.GetNextMessage(ref b))
			{
				try
				{
					bool flag = true;
					S_Message s_Message = MessageProcessor.ServerProcessMessage(this, this._messageInterceptor, nextMessage, ref flag);
					if (flag)
					{
						this.SendToClient((s_Message == null) ? nextMessage : s_Message.GetBuffer());
					}
				}
				catch (Exception)
				{
					this.SendToClient(nextMessage);
				}
			}
			this.ServerBeginReceive();
		}

		private void SendToServer(ByteBuffer bytesToSend)
		{
			if (!this._isRunning)
			{
				return;
			}
			if (bytesToSend.Count <= 255)
			{
				bytesToSend.PrependByte(this._clientFingerprint);
				bytesToSend.PrependByte((byte)(bytesToSend.Count - 1));
				bytesToSend.PrependByte(1);
			}
			else if (bytesToSend.Count <= 65535)
			{
				bytesToSend.PrependByte(this._clientFingerprint);
				bytesToSend.PrependShort((short)(bytesToSend.Count - 1));
				bytesToSend.PrependByte(2);
			}
			else
			{
				bytesToSend.PrependByte(this._clientFingerprint);
				bytesToSend.PrependMediumInt(bytesToSend.Count - 1);
				bytesToSend.PrependByte(3);
			}
			this.SendBytesToServer(bytesToSend.GetBytes());
			this._clientFingerprint += 1;
			this._clientFingerprint %= 100;
		}

		private void SendBytesToServer(byte[] bytesToSend)
		{
			try
			{
				this._serverSocket.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None, delegate(IAsyncResult x)
				{
					if (!x.IsCompleted || !(x.AsyncState is Socket))
					{
						this.Stop();
						return;
					}
					try
					{
						((Socket)x.AsyncState).EndSend(x);
					}
					catch (Exception)
					{
						this.Stop();
					}
				}, this._serverSocket);
			}
			catch (Exception)
			{
				this.Stop();
			}
		}
	}
}
