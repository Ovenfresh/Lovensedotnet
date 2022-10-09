using Newtonsoft.Json;
using System.Collections.Generic;

namespace Data.DTO
{
    public class FetchQRDTO
    {
        public bool Result { get; set; }
        public int Code { get; set; }
        [JsonProperty("message")]
        public string URL { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
