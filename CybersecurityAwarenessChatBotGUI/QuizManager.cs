using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessChatBotGUI
{
    public class QuizManager
    {
        public class Question
        {
            public string Text { get; set; }
            public List<string> Options { get; set; }
            public int CorrectIndex { get; set; }
            public string Explanation { get; set; }
            public string Type => Options.Count == 2 && Options[0].ToLower() == "true" ? "True/False" : "Multiple Choice";
        }

        private List<Question> questions;
        private int currentIndex = 0;
        private int score = 0;
        private List<bool> results = new List<bool>();
        private bool quizActive = false;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            questions = new List<Question>
            {
                // Multiple Choice Questions
                new Question
                {
                    Text = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report as phishing", "Ignore it" },
                    CorrectIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams and protects others."
                },
                new Question
                {
                    Text = "What is the best practice for creating a password?",
                    Options = new List<string> { "Use your pet's name", "Use 12+ characters with symbols and numbers", "Use 'password123'", "Use the same password everywhere" },
                    CorrectIndex = 1,
                    Explanation = "Strong passwords should be long, unique, and include a mix of characters."
                },
                new Question
                {
                    Text = "What does 2FA stand for?",
                    Options = new List<string> { "Two-Factor Authentication", "Second File Access", "Total File Access", "Temporary Access" },
                    CorrectIndex = 0,
                    Explanation = "Two-Factor Authentication adds an extra layer of security to your accounts."
                },
                new Question
                {
                    Text = "What is phishing?",
                    Options = new List<string> { "A type of virus", "A fraudulent attempt to obtain sensitive information", "A fishing technique", "A safe browsing method" },
                    CorrectIndex = 1,
                    Explanation = "Phishing is a cyberattack where scammers trick you into revealing personal information."
                },
                new Question
                {
                    Text = "What should you look for to identify a secure website?",
                    Options = new List<string> { "A padlock icon and 'https://'", "A green address bar", "No ads", "A '100% safe' badge" },
                    CorrectIndex = 0,
                    Explanation = "HTTPS and the padlock icon indicate the website connection is secure."
                },
                new Question
                {
                    Text = "How often should you update your passwords?",
                    Options = new List<string> { "Every 5 years", "Every 3-6 months", "Only when forced", "Never" },
                    CorrectIndex = 1,
                    Explanation = "Regular password updates help protect your accounts from unauthorized access."
                },
                new Question
                {
                    Text = "What is social engineering in cybersecurity?",
                    Options = new List<string> { "Building social networks", "Manipulating people to reveal information", "Engineering software", "Social media marketing" },
                    CorrectIndex = 1,
                    Explanation = "Social engineering attacks exploit human psychology rather than technical vulnerabilities."
                },

                // True/False Questions
                new Question
                {
                    Text = "True or False: It's safe to use public Wi-Fi without a VPN.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Public Wi-Fi is unencrypted. A VPN protects your data from being intercepted."
                },
                new Question
                {
                    Text = "True or False: You should share your password with trusted friends.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "You should never share your passwords - even with people you trust."
                },
                new Question
                {
                    Text = "True or False: A password manager helps you store and generate strong passwords.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 0,
                    Explanation = "Password managers are a recommended tool for maintaining strong, unique passwords."
                },
                new Question
                {
                    Text = "True or False: You should click links from unknown email senders.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Links in suspicious emails often lead to phishing websites."
                },
                new Question
                {
                    Text = "True or False: Two-Factor Authentication makes your account more secure.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 0,
                    Explanation = "2FA adds an additional verification step, making it harder for attackers."
                },
                new Question
                {
                    Text = "True or False: You should use the same password for multiple accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Using the same password across accounts increases your risk if one account is compromised."
                },
                new Question
                {
                    Text = "True or False: It's safe to download attachments from unknown sources.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 1,
                    Explanation = "Attachments from unknown sources can contain malware or viruses."
                },
                new Question
                {
                    Text = "True or False: Cookies can track your browsing habits across websites.",
                    Options = new List<string> { "True", "False" },
                    CorrectIndex = 0,
                    Explanation = "Third-party cookies are often used to track user behavior across the web."
                }
            };
        }

        public string StartQuiz()
        {
            currentIndex = 0;
            score = 0;
            results.Clear();
            quizActive = true;
            return GetCurrentQuestion();
        }

        public string GetCurrentQuestion()
        {
            if (!quizActive || currentIndex >= questions.Count)
                return null;

            var q = questions[currentIndex];
            string options = "";
            for (int i = 0; i < q.Options.Count; i++)
            {
                options += $"{i + 1}. {q.Options[i]}\n";
            }
            return $"📝 **Question {currentIndex + 1} of {questions.Count}**\n{q.Text}\n\n{options}Type the number of your answer.";
        }

        public string SubmitAnswer(string input)
        {
            if (!quizActive)
                return "❌ The quiz is not active. Type 'start quiz' to begin.";

            if (currentIndex >= questions.Count)
                return EndQuiz();

            if (!int.TryParse(input.Trim(), out int userChoice) || userChoice < 1 || userChoice > questions[currentIndex].Options.Count)
            {
                return "❌ Invalid input. Please enter the number of your choice.";
            }

            var q = questions[currentIndex];
            bool correct = (userChoice - 1) == q.CorrectIndex;
            if (correct) score++;
            results.Add(correct);

            string feedback = correct
                ? $"✅ Correct!\n"
                : $"❌ Incorrect.\n";

            feedback += q.Explanation;

            currentIndex++;

            if (currentIndex >= questions.Count)
            {
                return feedback + "\n\n" + EndQuiz();
            }

            return feedback + "\n\n" + GetCurrentQuestion();
        }

        public string EndQuiz()
        {
            quizActive = false;
            int total = results.Count;
            string result = $"\n\n📊 **Quiz Complete!**\nScore: {score}/{total}";

            double percentage = (double)score / total * 100;
            if (percentage >= 80)
                result += "\n🌟 Great job! You're a cybersecurity pro!";
            else if (percentage >= 60)
                result += "\n📚 Good effort! Keep learning to stay safe online.";
            else
                result += "\n🛡️ Keep learning! Cybersecurity is important for everyone.";

            return result;
        }

        public bool IsActive()
        {
            return quizActive;
        }

        public int GetQuestionCount()
        {
            return questions.Count;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public int GetScore()
        {
            return score;
        }
    }
}
