using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CybersecurityAwarenessChatBotGUI
{
    public class SenderToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sender = value as string;

            if (sender == "User")
            {
                // Return a style name for user messages (right-aligned)
                return "RightBubbleStyle";
            }
            else if (sender == "Bot")
            {
                // Return a style name for bot messages (left-aligned)
                return "LeftBubbleStyle";
            }

            return "LeftBubbleStyle"; // Default
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

