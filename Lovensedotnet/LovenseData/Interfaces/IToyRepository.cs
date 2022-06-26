using LovenseData.Models;
using System.Collections.Generic;

namespace LovenseData.Interfaces
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
