using Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Data.DTO
{
    public class ConnectToysDTO
    {
        public string Code { get; set; }
        [JsonProperty("data")]
        public Dictionary<string, Toy> Toys { get; set; }
        public string Type { get; set; }
    }
}
