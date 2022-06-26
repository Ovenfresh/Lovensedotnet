using LovenseData.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LovenseService.DTO
{
    public class RemoteDataDTO
    {
        [JsonProperty("Toys")]
        public string ParseIntoToys
        {
            set { Toys = JsonConvert.DeserializeObject<Dictionary<string, Toy>>(value); }
        }
        [JsonIgnore]
        public Dictionary<string, Toy> Toys;

        public string AppType { get; set; }
        [JsonProperty("gameAppId")]
        public string AppID { get; set; }
        public string Platform { get; set; }
    }
}