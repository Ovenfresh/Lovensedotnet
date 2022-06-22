using Newtonsoft.Json;

namespace Lovensedotnet.DTO
{
    public class RemoteToysDTO
    {
        public string Code { get; set; }
        [JsonProperty("data")]
        public RemoteDataDTO Data { get; set; }
        public string Type { get; set; }
    }
}
