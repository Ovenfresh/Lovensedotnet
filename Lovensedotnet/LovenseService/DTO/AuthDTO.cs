using Newtonsoft.Json;

namespace LovenseService.DTO
{
    public class AuthDTO
    {
        [JsonProperty("uid")]
        public string UserID { get; set; }
        [JsonProperty("token")]
        public string DevToken { get; set; }
        [JsonProperty("uname")]
        public string UserName { get; set; }
        [JsonProperty("uToken")]
        public string UserToken { get; set; }
        [JsonProperty("v")]
        public int ApiVer { get; set; } = 1;
    }
}
