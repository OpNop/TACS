using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TACSLib.Packets.Client;
using TinyLogger;

namespace TACS_Client
{
    public partial class Form1 : Form
    {
        private readonly UserSession us;
        private readonly Logger log;

        public Form1()
        {
            log = Logger.GetInstance();
            log.Initialize("client_", "", "./log", LogIntervalType.IT_PER_DAY, LogLevel.D, true, true);
            InitializeComponent();
            Console.SetOut(new ControlWriter(txtOutput));
            us = new UserSession(this);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                var rep = new IPEndPoint(IPAddress.Parse(txtAddress.Text), int.Parse(txtPort.Text));
                log.AddInfo($"Connecting to {rep.Address}:{rep.Port}");
                us.Connect(rep);
            } else
            {
                us.Stop();
            }

        }

        internal void updateButton(string message)
        {
            btnConnect.Invoke((MethodInvoker)delegate
            {
                btnConnect.Text = message;
            });
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            us.SendLogin(cbxAPI.SelectedValue.ToString());
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {
            txtOutput.ScrollToCaret();
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                var m = new ClientSendMessage(txtMessage.Text);
                us.SendEncrypted(m.Pack());
                txtMessage.Clear();
                e.Handled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbxAPI.DisplayMember = "Text";
            cbxAPI.ValueMember = "Value";

            var Keys = new[]
            {
                new { Text = "NullValue.4956", Value = "EAAD7DA6-3FB3-844A-BDB1-9B36252FBF2567D19728-7B43-4A0A-ABE9-B25BB0DC55DB"},
                new { Text = "OpNop.9453", Value = "07CBB6BD-653A-894B-A24D-69617DA1D3F024ACB22B-252C-4879-A8CA-0D5F014FC896"},
                new { Text = "FreeNop.5204", Value = "61DA3F3C-CB38-174E-9ADE-CC348E3429BCEFF34C02-E348-4D9F-BAC7-534EE4C1A848"},
                new { Text = "CosplayMuch.7234", Value = "C8C446BB-6822-2E4F-9B3A-DE56DC5EB3DF709A0F69-9D11-427F-811D-83C09AD25819"},
                new { Text = "eutaimi.5241", Value = "3B8DC320-AFC6-324A-A317-E382DBDEA1647C68D445-1EA6-4E9A-A922-1572FFD8E186" }
            };

            cbxAPI.DataSource = Keys;
        }

        private void btnChangerCharacter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAddress.Text))
            {
                var p = new ChangeCharacter(txtCharacterName.Text);
                us.SendEncrypted(p.Pack());
            }
        }

        private void btnSendKey_Click(object sender, EventArgs e)
        {
            us.SendKey();
        }
    }

    public class ControlWriter : TextWriter
    {
        private readonly TextBox textbox;
        public ControlWriter(TextBox textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            textbox.Invoke((MethodInvoker)delegate
             {
                 textbox.AppendText(value.ToString());
             });
        }

        public override void Write(string value)
        {
            textbox.Invoke((MethodInvoker)delegate
            {
                textbox.AppendText(value);
            });
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}
