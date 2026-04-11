using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    public class VoiceGreeting
    {
        private readonly string audioFilePath;

        public VoiceGreeting()
        {
            // Set path to the greeting.wav file in the application directory
            audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Welcome Audio WAV.wav");
            //C: \Users\DRUZA\source\repos\CybersecurityAwarenessBot\CybersecurityAwarenessBot\Welcome Audio WAV.wav
        }

        public void Play()
        {
            if (File.Exists(audioFilePath))
            {
                try
                {
                    using (SoundPlayer player = new SoundPlayer(audioFilePath))
                    {
                        player.PlaySync(); // Play synchronously to avoid cutting off
                    }
                }
                catch (Exception)
                {
                    // Silently fail if audio can't play - don't disrupt the user experience
                    Console.WriteLine("[Error playing audio.]");
                }
            }
            else
            {
                // No audio file found, show a friendly message instead
                Console.WriteLine("[Voice greeting file not found. Continuing with text mode.]");
            }
        }
    }
}

