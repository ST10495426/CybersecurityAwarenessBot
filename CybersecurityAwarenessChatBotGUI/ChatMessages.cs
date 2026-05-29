using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessChatBotGUI
{
    public class ChatMessages
    {
        public string Sender { get; set; }      // "User" or "Bot"
        public string Message { get; set; }     // The actual message text
        public DateTime TimeStamp { get; set; } // When message was sent

        // Property to determine alignment (User = right, Bot = left)
        public string Alignment
        {
            get
            {
                return Sender == "User" ? "Right" : "Left";
            }
        }

        // Property for background color (User = light green, Bot = light gray)
        public string BubbleColor
        {
            get
            {
                return Sender == "User" ? "#DCF8C6" : "#E4E6EB";
            }
        }

        // Constructor for easy creation
        public ChatMessages(string sender, string message)
        {
            Sender = sender;
            Message = message;
            TimeStamp = DateTime.Now;
        }
    }
}
