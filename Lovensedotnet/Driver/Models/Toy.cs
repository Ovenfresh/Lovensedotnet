using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Driver.Models
{
    public class Toy
    {
        [JsonProperty("nickname")]
        public string Name { get; set; }
        [JsonProperty("name")]
        public string User { get; set; }
        public string ID { get; set; }
        public int Status { get; set; }
    }
}
