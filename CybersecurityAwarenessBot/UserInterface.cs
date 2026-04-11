using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    //This class handles all the console display for the cybersecurity display
    public class UserInterface
    {
        public void DisplayWelcome()
        {
            DisplayColoredText("=== WELCOME TO THE CYBERSECURITY AWARENESS BOT ===", ConsoleColor.Cyan);
            Console.WriteLine();
        }

        public void DisplayPersonalisedWelcome(string userName)
        {
            DisplayColoredText($"Hello,"+userName+"!", ConsoleColor.Green);
            DisplayWithTypingEffect("I'm here to help you stay safe online. Type 'help' to see what I can do, or 'exit' to leave.\n", 30);
        }

        //public void DisplayHelpMessage()
        //{
       //     DisplayDivider();
        //    DisplayColoredText("  TIP: You can ask me about:", ConsoleColor.Yellow);
        //    DisplayColoredText("   • Password safety", ConsoleColor.White);
        //    DisplayColoredText("   • Phishing attacks", ConsoleColor.White);
        //    DisplayColoredText("   • Safe browsing habits", ConsoleColor.White);
        //    DisplayDivider();
        //    Console.WriteLine();
       // }

        public void DisplayBotResponse(string response)
        {
            if (response != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("CyberBot: ");
                Console.ResetColor();
                DisplayWithTypingEffect(response, 25);
            }
            else
            {
                DisplayDefaultResponse();
            }
        }

        public void DisplayDefaultResponse()
        {
            DisplayColoredText("I didn't quite understand that. Could you rephrase? Try asking about 'password safety', 'phishing', or 'safe browsing'.", ConsoleColor.Yellow);
        }

        public void DisplayExitMessage(string userName)
        {
            DisplayDivider();
            DisplayColoredText($"Goodbye,"+ userName+"! Stay safe online! ", ConsoleColor.Magenta);
            DisplayColoredText("Remember: Strong passwords, watch for phishing, and browse safely!", ConsoleColor.Green);
        }

        public void DisplayPrompt(string message)
        {
            DisplayColoredText(message, ConsoleColor.Yellow);
        }

        public void DisplayError(string message)
        {
            DisplayColoredText(message, ConsoleColor.Red);
        }

        public void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("--------------------------------------------------");
            Console.ResetColor();
        }

        public void DisplayWithTypingEffect(string text, int delayMilliseconds = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMilliseconds);
            }
            Console.WriteLine();
        }

        public void DisplayColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // Additional UI methods for enhanced visual experience
        public void ClearScreen()
        {
            Console.Clear();
        }

        public void DisplayBorder()
        {
            DisplayColoredText("╔════════════════════════════════════════════════════════════╗", ConsoleColor.DarkGray);
            DisplayColoredText("║                                                              ║", ConsoleColor.DarkGray);
            DisplayColoredText("╚════════════════════════════════════════════════════════════╝", ConsoleColor.DarkGray);
        }

        public void DisplayLoadingAnimation(int seconds = 2)
        {
            Console.Write("Loading");
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine();
        }
    }
}

