using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class UserRepositoryNoDB : IUserRepository
    {
        private Dictionary<string, Owner> Users { get; set; } = new();

        public void Add(Owner user)
        {
            if (!Users.ContainsKey(user.Name)) { Users.Add(user.Name, user); }
        }

        public bool CheckForUser(string id)
        {
            if (Users.ContainsKey(id)) { return true; }
            else { return false; }
        }

        public Owner GetUserByID(string id)
        {
            Users.TryGetValue(id, out Owner user);
            return user;
        }

        public void Update(Owner user)
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
