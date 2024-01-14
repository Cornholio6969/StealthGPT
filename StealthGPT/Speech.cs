using System.Speech.Synthesis;

namespace StealthGPT
{
    internal class Speech
    {
        public static void text2speech(string text)
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.Speak(text);
            }
        }
    }
}
