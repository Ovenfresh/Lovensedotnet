using LovenseData.Interfaces;
using LovenseData.Models;
using System.Collections.Generic;
using System.Text;

namespace LovenseData.Repositories
{
    public class UserRepositoryNoDB : IUserRepository
    {
        private Dictionary<string, User> Users { get; set; } = new();

        public void Add(User user)
        {
            if (!Users.ContainsKey(user.Name)) { Users.Add(user.Name, user); }
        }

        public bool CheckForUser(string id)
        {
            if (Users.ContainsKey(id)) { return true; }
            else { return false; }
        }

        public User GetUserByID(string id)
        {
            Users.TryGetValue(id, out User user);
            return user;
        }

        public void Update(User user)
        {
            Users[user.Name] = user;
        }
        public override string ToString()
        {
            var stb = new StringBuilder();
            foreach (var pair in Users)
            {
                stb.Append(pair.Key + ", ");
            }
            return stb.ToString();
        }
    }
}
