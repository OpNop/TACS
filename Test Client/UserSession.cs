using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TinyLogger;
using TinySocket.Server;
using TinySocket;
using TACSLib;
using TACSLib.Packets.Client;
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
            MessageBox.Show(e.Message);
        }

        protected override void OnReceived(byte[] receivedData)
        {
            PacketType type;
            Unpacker p;

            if (mRSAServer != null)
            {
                // Encrypted data
                var decryptedData = mRSASelf.Decrypt(receivedData, true);
                log.AddBinary("<RESV", decryptedData);
                p = new Unpacker(decryptedData);
                type = (PacketType)p.GetUInt8();
            }
            else
            {
                log.AddBinary("<RESV", receivedData);
                p = new Unpacker(receivedData);
                type = (PacketType)p.GetUInt8();
            }


            if(type == PacketType.S_SERVER_VER)
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
            var p = new Login(20201028, APIKey);
            SendEncrypted(p.Pack());
        }

        internal void SendKey()
        {
            Send(new SendCert(mRSASelf.ToXmlString(false)).Pack());
        }
    }
}
