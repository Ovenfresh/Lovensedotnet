using Newtonsoft.Json;

namespace Driver.Models
{
    public class Toy
    {
        [JsonProperty("nickname")]
        public string Alias { get; set; }
        [JsonProperty("name")]
        public string Model { get; set; }
        public string ID { get; set; }
        public int Status { get; set; }
    }
}
