using Newtonsoft.Json;

namespace Driver.DTO
{
    public class QRDTO
    {
        public bool Result { get; set; }
        public int Code { get; set; }
        [JsonProperty("message")]
        public string URL { get; set; }
    }
}
