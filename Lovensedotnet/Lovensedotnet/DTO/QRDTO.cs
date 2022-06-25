using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lovensedotnet.DTO
{
    public class QRDTO
    {
        public bool Result { get; set; }
        public int Code { get; set; }
        [JsonProperty("message")]
        public string URL { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
