using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiClient.DTO
{
    public class GetToysDTO
    {
        public string Code { get; set; }
        [JsonProperty("data")]
        public Dictionary<string, Toy> Toys{ get; set; }
        public string Type { get; set; }
    }
}
