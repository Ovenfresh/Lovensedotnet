using System.Collections.Generic;

namespace Lovensedotnet.Models
{
    public class User
    {
        public string UserID { get; set; }
        public Dictionary<string, Toy> Toys { get; set; }
        public LovenseApp App { get; set; }
        public int Port { get; set; }
        public string DeviceIP { get; set; }
    }
}
