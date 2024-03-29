﻿using Lovensedotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lovensedotnet.DTO
{
    public class DataDTO
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