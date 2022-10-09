using Data.Models;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        public Owner GetUserByID(string id);
        public bool CheckForUser(string id);
        public void Add(Owner user);
        public void Update(Owner user);
        public string ToString();
    }
}
