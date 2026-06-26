using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityAwarenessBot; // Reference to Part 1 classes


namespace CybersecurityAwarenessChatBotGUI
{
    public partial class MainWindow : Window
    {
        //part 3 classes
        private TaskManager taskManager;
        private QuizManager quizManager;
        private bool waitingForTaskAction = false;
        private string pendingTaskTitle = "";

        // Part 2 classes
        private EnhancedResponseManager responseManager;
        private SentimentCheck sentimentCheck;
        private ConversationMemory memory;

        // Part 1 classes
        private VoiceGreeting voiceGreeting;

        // User info
        private string userName = null;
        private bool waitingForName = false;
        private bool waitingForFirstMessage = true;
        private string currentTopic = null;
        private string lastAskedTopic = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();

            // Play voice greeting after window is loaded
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try { voiceGreeting.Play(); } catch { }
        }

        private void InitializeChatbot()
        {
            // Initialize Part 2 classes
            responseManager = new EnhancedResponseManager();
            sentimentCheck = new SentimentCheck();
            memory = new ConversationMemory();

            // Initialize Part 1 classes
            voiceGreeting = new VoiceGreeting();

            // Bot waits for user to initiate conversation
            waitingForFirstMessage = true;

            // Load ASCII art from text file
            try
            {
                string asciiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "ASCII ART.txt");
                if (System.IO.File.Exists(asciiPath))
                {
                    AsciiArtDisplay.Text = System.IO.File.ReadAllText(asciiPath);
                }
                else
                {
                    AsciiArtDisplay.Text = "|| Cybersecurity Awareness ChatBot ||";
                }
            }
            catch { }

            InitializePart3();
        }
            private void InitializePart3()
        {
            taskManager = new TaskManager();
            quizManager = new QuizManager();
            ActivityLogger.Log("System", "Chatbot started");
        }
        
        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            string summary = taskManager.GetTasksSummary();
            AddBotMessage(" " + summary);
            ActivityLogger.Log("User Action", "Viewed tasks");
        }

        private void btnQuiz_Click(object sender, RoutedEventArgs e)
        {
            string startMsg = quizManager.StartQuiz();
            AddBotMessage(startMsg);
            ActivityLogger.Log("User Action", "Started quiz");
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            string log = ActivityLogger.GetLog();
            AddBotMessage(log);
            ActivityLogger.Log("User Action", "Viewed activity log");
        }

        private void AddBotMessage(string message)
        {
            AddMessageToChat("CyberBot", message);
        }

        private void AddUserMessage(string message)
        {
            string displayName = string.IsNullOrEmpty(userName) ? "You" : userName;
            AddMessageToChat(displayName, message);
        }

        private void AddMessageToChat(string sender, string message)
        {
            // Create border for message bubble
            Border bubble = new Border();
            bubble.CornerRadius = new CornerRadius(10);
            bubble.Padding = new Thickness(12, 8, 12, 8);
            bubble.MaxWidth = 380;

            // Set alignment and color based on sender
            if (sender == userName || sender == "You")
            {
                // User message - Right side, green bubble
                bubble.Background = new SolidColorBrush(Color.FromRgb(220, 248, 198));
                bubble.HorizontalAlignment = HorizontalAlignment.Right;
                bubble.Margin = new Thickness(50, 5, 5, 5);
            }
            else
            {
                // Bot message - Left side, gray bubble
                bubble.Background = new SolidColorBrush(Color.FromRgb(228, 230, 235));
                bubble.HorizontalAlignment = HorizontalAlignment.Left;
                bubble.Margin = new Thickness(5, 5, 50, 5);
            }

            // Create content stack
            StackPanel content = new StackPanel();

            // Sender name (show for bot, and for user if name is known)
            if (sender != "You")
            {
                TextBlock nameText = new TextBlock();
                nameText.Text = sender;
                nameText.FontSize = 11;
                nameText.FontWeight = FontWeights.Bold;
                nameText.Foreground = new SolidColorBrush(Color.FromRgb(0, 94, 84));
                nameText.Margin = new Thickness(0, 0, 0, 3);
                content.Children.Add(nameText);
            }

            // Message text
            TextBlock messageText = new TextBlock();
            messageText.Text = message;
            messageText.TextWrapping = TextWrapping.Wrap;
            messageText.FontSize = 13;
            messageText.Foreground = new SolidColorBrush(Color.FromRgb(17, 17, 17));
            content.Children.Add(messageText);

            // Timestamp
            TextBlock timestamp = new TextBlock();
            timestamp.Text = DateTime.Now.ToString("HH:mm tt");
            timestamp.FontSize = 9;
            timestamp.Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102));
            timestamp.HorizontalAlignment = HorizontalAlignment.Right;
            timestamp.Margin = new Thickness(0, 5, 0, 0);
            content.Children.Add(timestamp);

            bubble.Child = content;
            ChatPanel.Children.Add(bubble);

            // Scroll to bottom
            chatScrollViewer.ScrollToBottom();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void txtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift))
            {
                ProcessUserInput();
                e.Handled = true;
            }
        }

        private void ProcessUserInput()
        {
            string input = txtUserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
                return;

            // Show user message
            AddUserMessage(input);

            // Handle first message (user initiates conversation)
            if (waitingForFirstMessage)
            {
                waitingForFirstMessage = false;
                waitingForName = true;
                AddBotMessage("Hello! Welcome to Cybersecurity Awareness Bot. I'm your Cybersecurity Assistant.Please tell me your name");
                txtUserInput.Clear();
                return;
            }

            // Handle name collection
            if (waitingForName)
            {
                userName = input;
                waitingForName = false;
                memory.Remember("name", userName);
                AddBotMessage($"Hello, {userName}! Hope you are good.How can I help you with cybersecurity today? ");
                
                txtUserInput.Clear();
                return;
            }
            // Check if quiz is active
            if (quizManager.IsActive())
            {
                string quizResponse = quizManager.SubmitAnswer(input);
                AddBotMessage(quizResponse);
                ActivityLogger.Log("Quiz", $"Answered question {quizManager.GetCurrentIndex()}/{quizManager.GetQuestionCount()}");
                txtUserInput.Clear();
                return;
            }

            // Check for exit
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                ExitApplication();
                return;
            }

            // ============ NLP TASK DETECTOR ============
            string taskIntent = responseManager.DetectTaskIntent(input);

            if (taskIntent != null)
            {
                switch (taskIntent)
                {
                    case "add_task":
                        string taskTitle = responseManager.ExtractTaskTitle(input);
                        pendingTaskTitle = taskTitle;
                        waitingForTaskAction = true;
                        AddBotMessage($"Task detected: '{taskTitle}'. Would you like to add a reminder? (yes/no)");
                        ActivityLogger.Log("NLP", $"Task detected: {taskTitle}");
                        txtUserInput.Clear();
                        return;
                    case "view_tasks":
                        string summary = taskManager.GetTasksSummary();
                        AddBotMessage(" " + summary);
                        ActivityLogger.Log("User Action", "Viewed tasks via NLP");
                        txtUserInput.Clear();
                        return;
                    case "set_reminder":
                        string reminderTask = responseManager.ExtractTaskTitle(input);
                        string reminderTime = responseManager.ExtractReminderTime(input);
                        DateTime reminderDate = DateTime.Now.AddDays(1);
                        if (reminderTime != null)
                        {
                            int days = int.Parse(reminderTime.Split(' ')[0]);
                            reminderDate = DateTime.Now.AddDays(days);
                        }
                        taskManager.AddTask(reminderTask, "", reminderDate);
                        AddBotMessage($"Reminder set for '{reminderTask}' on {reminderDate:yyyy-MM-dd HH:mm}");
                        ActivityLogger.Log("Task", $"Reminder set: {reminderTask} - {reminderDate}");
                        txtUserInput.Clear();
                        return;
                }
            }

            // Handle "yes" response to task reminder prompt
            if (waitingForTaskAction && input.ToLower() == "yes")
            {
                waitingForTaskAction = false;
                AddBotMessage("Please specify the reminder time (e.g., 'in 3 days' or 'tomorrow'):");
                txtUserInput.Clear();
                return;
            }
            if (waitingForTaskAction && input.ToLower() == "no")
            {
                waitingForTaskAction = false;
                taskManager.AddTask(pendingTaskTitle, "", null);
                AddBotMessage($"Task '{pendingTaskTitle}' added with no reminder.");
                ActivityLogger.Log("Task", $"Added: {pendingTaskTitle} (no reminder)");
                txtUserInput.Clear();
                return;
            }
            if (waitingForTaskAction)
            {
                // User specified a reminder time
                string reminderTime = input.ToLower();
                DateTime reminderDate = DateTime.Now.AddDays(1);
                if (reminderTime.Contains("tomorrow"))
                    reminderDate = DateTime.Now.AddDays(1);
                else if (reminderTime.Contains("3 days"))
                    reminderDate = DateTime.Now.AddDays(3);
                else if (reminderTime.Contains("5 days"))
                    reminderDate = DateTime.Now.AddDays(5);
                else if (reminderTime.Contains("7 days") || reminderTime.Contains("week"))
                    reminderDate = DateTime.Now.AddDays(7);
                else if (reminderTime.Contains("2 days"))
                    reminderDate = DateTime.Now.AddDays(2);

                taskManager.AddTask(pendingTaskTitle, "", reminderDate);
                AddBotMessage($"Task '{pendingTaskTitle}' added with reminder for {reminderDate:yyyy-MM-dd}");
                ActivityLogger.Log("Task", $"Added: {pendingTaskTitle} with reminder on {reminderDate}");
                waitingForTaskAction = false;
                txtUserInput.Clear();
                return;
            }


        // Normal conversation
        string response = GenerateResponse(input);
            AddBotMessage(response);

            // Store in memory
            memory.AddToHistory($"User: {input}");
            memory.AddToHistory($"Bot: {response}");

            txtUserInput.Clear();
        }

        private string GenerateResponse(string input)
        {
            string lowerInput = input.ToLower();
            string userInterest = memory.GetUserInterest();

            // ============ PART 1 RESPONSES ============

            // How are you
            if (lowerInput.Contains("how are you"))
            {
                return $"I'm functioning securely, thank you for asking! How can I help you stay safe online, {userName}?";
            }

            // What's your purpose
            if (lowerInput.Contains("what's your purpose") || lowerInput.Contains("what is your purpose"))
            {
                return "My purpose is to educate and assist you with cybersecurity best practices. I'm CyberBot, your personal security assistant!";
            }

            // What can I ask about
            if (lowerInput.Contains("what can i ask you about") || lowerInput.Contains("what can i ask"))
            {
                return "You can ask me about:\n• Password safety\n• Phishing attacks\n• Safe browsing habits\n• Scam protection\n• Privacy tips";
            }

            // Help
            if (lowerInput.Contains("help"))
            {
                string keywords = string.Join(", ", responseManager.GetAvailableKeywords());
                return $"Here's what I can help you with, {userName}:\n\n{keywords}\n\nYou can also ask:\n• How are you?\n• What's your purpose?\n• Tell me more (for additional tips)\n• Another tip";
            }

            // ============ PART 2 FEATURES ============

            // Check if user says they are "interested" in a topic
            if (lowerInput.Contains("interested in"))
            {
                int index = lowerInput.IndexOf("interested in") + 13;
                if (index < lowerInput.Length)
                {
                    string interestedTopic = lowerInput.Substring(index).Trim();
                    string matchedKeyword = responseManager.DetectKeyword(interestedTopic);
                    if (matchedKeyword != null)
                    {
                        memory.Remember("interest", matchedKeyword);
                        string tip = responseManager.GetRandomResponse(matchedKeyword);
                        return $"Great! I'll remember that you're interested in {matchedKeyword}. " + tip + $"\n\nSince you're interested in {matchedKeyword}, would you like another tip?";
                    }
                }
            }

            // Check if user wants more info
            if (memory.WantsMoreInfo(input))
            {
                // ALWAYS use currentTopic (the latest topic discussed)
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    string moreInfo = responseManager.GetRandomResponse(currentTopic);
                    return moreInfo + $"\n\nWould you like another tip about {currentTopic}, {userName}?";
                }
                else
                {
                    return $"What topic would you like another tip about, {userName}? ";
                }
            }

            // ============ KEYWORD DETECTION  ===========

            string detectedKeyword = responseManager.DetectKeyword(input);

            // Force detect if input is a single word that matches
            if (detectedKeyword == null && !string.IsNullOrWhiteSpace(input))
            {
                string trimmed = input.Trim().ToLower();
                foreach (string keyword in responseManager.GetAvailableKeywords())
                {
                    if (trimmed.Contains(keyword) || keyword.Contains(trimmed))
                    {
                        detectedKeyword = keyword;
                        break;
                    }
                }
            }

            if (detectedKeyword != null)
            {
                bool isFirstTimeOnTopic = (currentTopic != detectedKeyword);
                currentTopic = detectedKeyword;
                memory.SetLastTopic(detectedKeyword);

                // Store interest if not already stored
                if (memory.GetUserInterest() == null)
                {
                    memory.Remember("interest", detectedKeyword);
                }

                // Get sentiment AFTER detecting the keyword
                string sentiment = sentimentCheck.DetectSentiment(input);

                // Handle sentiment for THIS topic
                if (sentiment == "worried")
                {
                    string tip = responseManager.GetRandomResponse(detectedKeyword);
                    return $"I understand your concern about {detectedKeyword}. " + tip + $"\n\nWould you like another tip?";
                }
                else if (sentiment == "curious")
                {
                    string tip = responseManager.GetRandomResponse(detectedKeyword);
                    return $"That's great that you're curious about {detectedKeyword}! " + tip + $"\n\nWould you like to learn more?";
                }
                else if (sentiment == "frustrated")
                {
                    string tip = responseManager.GetRandomResponse(detectedKeyword);
                    return $"I know cybersecurity can be frustrating. Let me simplify this for you. " + tip + $"\n\nDoes that make more sense?";
                }
                else if (sentiment == "happy")
                {
                    string tip = responseManager.GetRandomResponse(detectedKeyword);
                    return $"I'm glad you're feeling positive about cybersecurity! " + tip;
                }

                // Normal response (no special sentiment)
                string greeting = "";
                if (isFirstTimeOnTopic && new Random().Next(3) == 0)
                {
                    string[] greetings = { "Great question, ", "Good question, ", "Thanks for asking, " };
                    greeting = greetings[new Random().Next(greetings.Length)] + userName + "! ";
                }

                string response = responseManager.GetRandomResponse(detectedKeyword);

                string memoryMsg = "";
                string storedInterest = memory.GetUserInterest();
                if (storedInterest != null && storedInterest != detectedKeyword && new Random().Next(2) == 0)
                {
                    memoryMsg = $"\n\n(Since you're interested in {storedInterest}, let me know if you'd like tips on that too!)";
                }

                return greeting + response + memoryMsg + $"\n\nWould you like another tip about {detectedKeyword}?";
            }
            // Greetings
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                if (userInterest != null)
                {
                    return $"Hello again, {userName}! Since you're interested in {userInterest}, would you like more tips on that topic?";
                }
                return $"Hello, {userName}! How can I help you with cybersecurity today?";
            }

            // DEFAULT RESPONSE 
            return $"I'm not sure I understand, {userName}. Can you try rephrasing?\n\nTry asking about:\n• password\n• scam\n• privacy\n• phishing\n• browsing\n\nOr type 'help' to see all options.";
        }
        //Exit method
        private void ExitApplication()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                AddBotMessage("Goodbye! Stay safe online! ");

                // Wait 1 second so user sees goodbye message
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, e) => { timer.Stop(); Application.Current.Shutdown(); };
                timer.Start();
            }
        }

    }
}