using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TinyLogger;
using TinySocket.Server;
using TinySocket;
using TACSLib.Packets;
using System.Security.Cryptography;
using System.Linq;

namespace TACS_Client
{
    public class UserSession : ASocket
    {
        private Form1 form;
        private RSACryptoServiceProvider mRSASelf;
        private RSACryptoServiceProvider mRSAServer;
        private Logger log = Logger.GetInstance();

        public UserSession(Form1 form)
        {
            this.form = form;
            mRSASelf = new RSACryptoServiceProvider(2048);
        }
        protected override void OnConnected()
        {
            form.updateButton("Disconnect");
            log.AddInfo("Connected");
        }

        protected override void OnConnectFailed(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        protected override void OnDisconnected(Exception e)
        {
            form.updateButton("Connect");
            //MessageBox.Show(e.Message);
        }

        protected override void OnReceived(byte[] receivedData)
        {
            log.AddBinary("<RESV", receivedData);
            var p = new Unpacker(receivedData);
            TACSLib.PacketType type = (TACSLib.PacketType)p.GetUInt8();
            if(type == TACSLib.PacketType.S_SERVER_VER)
            {
                mRSAServer = new RSACryptoServiceProvider();
                mRSAServer.FromXmlString(p.GetString());
            }
        }

        protected override void OnSendFailed(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        public override void Send(byte[] packetBody)
        {
            log.AddBinary("SEND>", packetBody);
            base.Send(packetBody);
        }

        public void SendEncrypted(byte[] packetBody)
        {
            log.AddBinary("SEND>", packetBody);
            base.Send(mRSAServer.Encrypt(packetBody, true));
        }

        public void Connect(EndPoint remoteEndPoint)
        {
            BeginConnect(remoteEndPoint);
        }

        internal void SendLogin(string APIKey)
        {
            var p = new Login()
            {
                APIKey = APIKey,
                ClientVersion = 20201028
            };
            SendEncrypted(p.Pack());
        }

        internal void SendKey()
        {
            Send(new SendCert(mRSASelf.ToXmlString(false)).Pack());
        }
    }
}
