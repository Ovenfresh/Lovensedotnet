using Data.Models;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IToyRepository
    {
        public void Add(Toy toy);
        public Toy GetToyAtIndex(int index);
        public Toy GetToyByID(string id);
        public List<Toy> GetAll();
        void Remove(Toy toy);
    }
}
