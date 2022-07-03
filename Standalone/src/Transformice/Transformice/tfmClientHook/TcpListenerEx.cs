using System;
using System.Net;
using System.Net.Sockets;

namespace Transformice.tfmClientHook
{
	internal sealed class TcpListenerEx : TcpListener
	{
		public int Port { get; }
		public new bool Active
		{
			get
			{
				return base.Active;
			}
		}
		
		public TcpListenerEx(IPAddress localAddress, int port) : base(localAddress, port)
		{
			this.Port = port;
		}
	}
}
