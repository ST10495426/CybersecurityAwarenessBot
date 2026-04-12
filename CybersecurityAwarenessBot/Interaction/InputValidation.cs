using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    public class InputValidation
    {
        private readonly UserInterface userInterface;  

        public InputValidation()
        {
            userInterface = new UserInterface();  
        }

        public string GetValidatedName(string prompt)
        {
            string name = "";

            while (string.IsNullOrWhiteSpace(name))
            {
                userInterface.DisplayPrompt(prompt);
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    userInterface.DisplayError("Name cannot be empty. Please enter a valid name.");
                }
            }

            return name.Trim();
        }

        public string GetUserInput(string userName)
        {
            Console.Write($"{userName}: ");
            string input = Console.ReadLine();

            // Input validation which handle empty entries
            if (string.IsNullOrWhiteSpace(input))
            {
                userInterface.DisplayDefaultResponse();
                return GetUserInput(userName); //  ask repeatedly
            }

            return input;
        }

        // Additional validation methods 
        public bool IsValidYesNo(string input)
        {
            return input.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("n", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}