using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    public class UserSession
    {
        // Automatic property for current user
        public string CurrentUser { get; private set; }

        public UserSession()
        {
            CurrentUser = "User";
        }

        public void SetCurrentUser(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                CurrentUser = name.Trim();
            }
        }
    }
}
