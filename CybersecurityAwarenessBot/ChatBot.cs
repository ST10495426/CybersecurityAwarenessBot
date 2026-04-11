using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    using System;

    namespace CybersecurityAwarenessBot
    {
        public class ChatBot
        {
           
            private VoiceGreeting voiceGreeting;
            private InputValidation inputValidation;
            private ResponseManager responseManager;
            private UserInterface userInterface;  
            private UserSession userSession;

            public ChatBot()
            {
                
                voiceGreeting = new VoiceGreeting();
                inputValidation = new InputValidation();
                responseManager = new ResponseManager();
                userInterface = new UserInterface();  
                userSession = new UserSession();
            }

            public void Start()
            {
                // Play voice greeting
                voiceGreeting.Play();


                // Show welcome message
                userInterface.DisplayWelcome();

                // Get user's name with validation
                string userName = inputValidation.GetValidatedName("Please tell me your name:");
                userSession.SetCurrentUser(userName);

                // Personalised welcome
                userInterface.DisplayPersonalisedWelcome(userName);
               // userInterface.DisplayHelpMessage();

                // Main conversation loop
                RunConversationLoop();

                // Exit message
                userInterface.DisplayExitMessage(userName);
            }

            private void RunConversationLoop()
            {
                bool running = true;

                while (running)
                {
                    // Get user input
                    string userInput = inputValidation.GetUserInput(userSession.CurrentUser);

                    // Check for exit command
                    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        running = false;
                        continue;
                    }

                    // Get response from the bot
                    string response = responseManager.GetResponse(userInput, userSession.CurrentUser);

                    // Display the response
                    userInterface.DisplayBotResponse(response);

                    // Add visual separator
                    userInterface.DisplayDivider();
                }
            }
        }
    }
}
