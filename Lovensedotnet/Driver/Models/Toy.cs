using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Driver.Models
{
    public class Toy
    {
        public string ID { get; set; }
        [JsonProperty("nickname")]
        public string? Alias { get; set; }
        [JsonProperty("name")]
        public string Model { get; set; }
        public string? Version { get; set; }
        public int Status { get; set; }
        public int? Battery { get; set; }
        [JsonProperty("fVersion")]
        public string? FirmwareVersion { get; set; }
    }
}
