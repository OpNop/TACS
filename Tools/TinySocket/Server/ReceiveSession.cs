namespace TinySocket.Server
{
    public class ReceiveSession
    {
        public byte[] Buffer;
        public int ReceiveSize;
        public SessionState State;

        public ReceiveSession(SessionState state, int buffer_size)
        {
            State = state;
            Buffer = new byte[buffer_size];
            ReceiveSize = 0;
        }
    }
}

