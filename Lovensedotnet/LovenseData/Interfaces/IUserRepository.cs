using LovenseData.Models;

namespace LovenseData.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserByID(string id);
        public bool CheckForUser(string id);
        public void Add(User user);
        public void Update(User user);
        public string ToString();
    }
}
