using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TinySocket.Server
{

    public abstract class ASocket
    {
        public enum SendStatus
        {
            Ready,
            Sending
        }

        private static int mTotalQueueCount;
        private static int mTotalThreadCount;
        private readonly Queue mQueue = new Queue();
        private SendStatus mSendStatus;
        private Socket mSocket;

        public bool Connected
        {
            get
            {
                if (mSocket != null)
                {
                    return mSocket.Connected;
                }
                return false;
            }
        }
        public EndPoint LocalEndPoint
        {
            get
            {
                if (mSocket != null)
                {
                    return mSocket.LocalEndPoint;
                }
                return null;
            }
        }
        public int QueueCount => mQueue.Count;
        public EndPoint RemoteEndPoint
        {
            get
            {
                if (mSocket != null)
                {
                    return mSocket.RemoteEndPoint;
                }
                return null;
            }
        }
        public static int TotalQueueCount => mTotalQueueCount;
        public static int TotalThreadCount => mTotalThreadCount;

        private void AsyncConnectCallback(IAsyncResult ar)
        {
            try
            {
                mSocket.EndConnect(ar);
                BeginReceive(mSocket);
            }
            catch (Exception e)
            {
                OnConnectFailed(e);
            }
        }

        private void AsyncReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Interlocked.Increment(ref mTotalThreadCount);
                ReceiveSession receiveSession = (ReceiveSession)ar.AsyncState;
                int num = mSocket.EndReceive(ar);
                if (num == 0)
                {
                    throw new Exception("EndReceive(...) Returned 0.");
                }
                receiveSession.ReceiveSize += num;
                if (receiveSession.ReceiveSize < receiveSession.Buffer.Length)
                {
                    mSocket.BeginReceive(receiveSession.Buffer, receiveSession.ReceiveSize, receiveSession.Buffer.Length - receiveSession.ReceiveSize, SocketFlags.None, AsyncReceiveCallback, receiveSession);
                }
                else
                {
                    if (receiveSession.State == SessionState.Header)
                    {
                        int num2 = BitConverter.ToInt16(receiveSession.Buffer, 0) + 1;
                        if (0 < num2)
                        {
                            receiveSession = new ReceiveSession(SessionState.Body, num2);
                        }
                        else
                        {
                            OnReceived(Array.Empty<byte>());
                            receiveSession = new ReceiveSession(SessionState.Header, 2);
                        }
                    }
                    else
                    {
                        OnReceived(receiveSession.Buffer);
                        receiveSession = new ReceiveSession(SessionState.Header, 2);
                    }
                    mSocket.BeginReceive(receiveSession.Buffer, 0, receiveSession.Buffer.Length, SocketFlags.None, AsyncReceiveCallback, receiveSession);
                }
            }
            catch (Exception e)
            {
                lock (mQueue)
                {
                    mTotalQueueCount -= mQueue.Count;
                    mQueue.Clear();
                    mSendStatus = SendStatus.Ready;
                }
                OnDisconnected(e);
            }
            finally
            {
                Interlocked.Decrement(ref mTotalThreadCount);
            }
        }

        private void AsyncSendCallback(IAsyncResult ar)
        {
            try
            {
                Interlocked.Increment(ref mTotalThreadCount);
                if (!Connected)
                {
                    lock (mQueue)
                    {
                        mTotalQueueCount -= mQueue.Count;
                        mQueue.Clear();
                        mSendStatus = SendStatus.Ready;
                    }
                }
                else
                {
                    SendSession sendSession = (SendSession)ar.AsyncState;
                    int num = mSocket.EndSend(ar);
                    if (num == 0)
                    {
                        throw new Exception("EndSend(...) Returned 0.");
                    }
                    sendSession.SentSize += num;
                    if (sendSession.SentSize < sendSession.Buffer.Length)
                    {
                        mSocket.BeginSend(sendSession.Buffer, sendSession.SentSize, sendSession.Buffer.Length - sendSession.SentSize, SocketFlags.None, AsyncSendCallback, sendSession);
                    }
                    else
                    {
                        lock (mQueue)
                        {
                            if (0 < mQueue.Count)
                            {
                                sendSession = (SendSession)mQueue.Dequeue();
                                Interlocked.Decrement(ref mTotalQueueCount);
                                mSocket.BeginSend(sendSession.Buffer, 0, sendSession.Buffer.Length, SocketFlags.None, AsyncSendCallback, sendSession);
                            }
                            else
                            {
                                mSendStatus = SendStatus.Ready;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                OnSendFailed(e);
            }
            finally
            {
                Interlocked.Decrement(ref mTotalThreadCount);
            }
        }

        public void BeginConnect(EndPoint remoteEndPoint)
        {
            try
            {
                Interlocked.Increment(ref mTotalThreadCount);
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mSocket.BeginConnect(remoteEndPoint, AsyncConnectCallback, this);
            }
            catch (Exception e)
            {
                OnConnectFailed(e);
            }
            finally
            {
                Interlocked.Decrement(ref mTotalThreadCount);
            }
        }

        public void BeginReceive(Socket socket)
        {
            try
            {
                mSocket = socket;
                lock (mQueue)
                {
                    mTotalQueueCount -= mQueue.Count;
                    mQueue.Clear();
                    mSendStatus = SendStatus.Ready;
                }
                OnConnected();
                ReceiveSession receiveSession = new ReceiveSession(SessionState.Header, 2);
                mSocket.BeginReceive(receiveSession.Buffer, 0, receiveSession.Buffer.Length, SocketFlags.None, AsyncReceiveCallback, receiveSession);
            }
            catch (Exception e)
            {
                OnConnectFailed(e);
            }
        }

        protected abstract void OnConnected();

        protected abstract void OnConnectFailed(Exception e);

        protected abstract void OnDisconnected(Exception e);

        protected abstract void OnReceived(byte[] receivedData);

        protected abstract void OnSendFailed(Exception e);

        public virtual void Send(byte[] packetBody)
        {
            try
            {
                if (!Connected)
                {
                    lock (mQueue)
                    {
                        mTotalQueueCount -= mQueue.Count;
                        mQueue.Clear();
                        mSendStatus = SendStatus.Ready;
                    }
                }
                else
                {
                    SendSession sendSession = null;
                    SendSession sendSession2 = null;
                    byte[] bytes = BitConverter.GetBytes((short)(packetBody.Length - 1));
                    sendSession = new SendSession(SessionState.Header, bytes);
                    if (packetBody.Length != 0)
                    {
                        sendSession2 = new SendSession(SessionState.Body, packetBody);
                    }
                    lock (mQueue)
                    {
                        mQueue.Enqueue(sendSession);
                        Interlocked.Increment(ref mTotalQueueCount);
                        if (sendSession2 != null)
                        {
                            mQueue.Enqueue(sendSession2);
                            Interlocked.Increment(ref mTotalQueueCount);
                        }
                        if (mSendStatus == SendStatus.Ready)
                        {
                            mSendStatus = SendStatus.Sending;
                            SendSession sendSession3 = (SendSession)mQueue.Dequeue();
                            Interlocked.Decrement(ref mTotalQueueCount);
                            mSocket.BeginSend(sendSession3.Buffer, 0, sendSession3.Buffer.Length, SocketFlags.None, AsyncSendCallback, sendSession3);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                OnSendFailed(e);
            }
        }

        public void Stop()
        {
            try
            {
                if (Connected)
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    mSocket.Close();
                }
            }
            catch
            {
            }
        }
    }

}