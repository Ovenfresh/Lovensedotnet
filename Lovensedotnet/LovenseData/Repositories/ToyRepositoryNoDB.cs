using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class ToyRepositoryNoDB : IToyRepository
    {
        private Dictionary<string, Toy> Toys { get; set; } = new();
        public void Add(Toy toy)
        {
            if (!Toys.ContainsKey(toy.ID))
            {
                Toys.Add(toy.ID, toy);
            }
        }

        public List<Toy> GetAll()
        {
            return Toys.Values.ToList();
        }

        public Toy GetToyAtIndex(int index)
        {
            return Toys.ElementAt(index).Value;
        }

        public Toy GetToyByID(string id)
        {
            Toys.TryGetValue(id, out Toy toy);
            return toy;
        }

        public void Remove(Toy toy)
        {
            Toys.Remove(toy.ID);
        }
    }
}
