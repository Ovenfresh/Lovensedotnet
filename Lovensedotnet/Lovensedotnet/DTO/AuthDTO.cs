using Newtonsoft.Json;

namespace Lovensedotnet.DTO
{
    public class AuthDTO
    {
        [JsonProperty("uid")]
        public string UserID { get; set; }
        [JsonProperty("token")]
        public string DevToken { get; set; }
        public string User { get; set; }
        public string UserAuth { get; set; }
        [JsonProperty("v")]
        public int Version { get; set; } = 1;
    }
}
