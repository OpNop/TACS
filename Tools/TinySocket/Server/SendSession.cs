namespace TinyTools.TinySocket.Server
{
    public class SendSession
    {
        public byte[] Buffer;
        public int SentSize;
        public SessionState State;

        public SendSession(SessionState state, byte[] buffer)
        {
            State = state;
            Buffer = buffer;
            SentSize = 0;
        }
    }
}

