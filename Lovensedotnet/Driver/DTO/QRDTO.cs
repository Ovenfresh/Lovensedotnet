using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.DTO
{
    public class QRDTO
    {
        public bool Result { get; set; }
        public int Code{ get; set; }
        [JsonProperty("message")]
        public string URL{ get; set; }
    }
}
