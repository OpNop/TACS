using System;
using System.Net;
using System.Net.Sockets;

namespace TinySocket.Server
{
    public class ListenerSocket
    {
        public delegate void AcceptedHandler(ListenerSocket listenerSocket, Socket socket);

        private Socket mSocket;

        public EndPoint LocalEndPoint
        {
            get
            {
                if (mSocket == null)
                {
                    return null;
                }
                return mSocket.LocalEndPoint;
            }
        }

        public event AcceptedHandler Accepted;

        private void AsyncAcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = mSocket.EndAccept(ar);
                if (this.Accepted != null)
                {
                    Accepted(this, socket);
                }
                mSocket.BeginAccept(AsyncAcceptCallback, this);
            }
            catch
            {
            }
        }

        public void Start(EndPoint localEndPoint)
        {
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mSocket.Bind(localEndPoint);
            mSocket.Listen(5);
            mSocket.BeginAccept(AsyncAcceptCallback, this);
        }

        public void Stop()
        {
            try
            {
                mSocket.Close();
            }
            catch
            {
            }
        }
    }

}

