using NAudio.Wave;
using Server.Forms;

namespace Server.Handle_Packet
{
    public class AudioListener
    {
        private static WaveOutEvent waveOut;
        private static BufferedWaveProvider bufferedWaveProvider;

        static AudioListener()
        {
            InitializeAudio();
        }

        private static void InitializeAudio()
        {
            FormMicrophone mic = new FormMicrophone();

            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(mic.GlobalAudioRate, 16, mic.GlobalAudioChannels))
            {
                DiscardOnBufferOverflow = true
            };
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();
        }

        public static void PlayAudio(byte[] audiobytes)
        {
            if (audiobytes == null || audiobytes.Length == 0) return;

            if (bufferedWaveProvider == null || waveOut == null)
            {
                InitializeAudio();
            }

            bufferedWaveProvider.AddSamples(audiobytes, 0, audiobytes.Length);
        }

        public static void StopAudio()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
            waveOut = null;
            bufferedWaveProvider = null;
        }

    }
}