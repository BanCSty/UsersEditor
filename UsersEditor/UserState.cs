using System.Collections.Concurrent;

namespace UsersEditor
{
    public class UserState
    {
        private static readonly ConcurrentDictionary<string, bool> _users = new();

        public void addOnlineUser(string mail)
        {
            _users[mail] = true;
        }

        public void removeOnlineUser(string mail)
        {
            _users[mail] = false;
        }

        public static List<string> getOnlineUsers()
        {
            return _users.Keys.ToList();
        }
    }
}
