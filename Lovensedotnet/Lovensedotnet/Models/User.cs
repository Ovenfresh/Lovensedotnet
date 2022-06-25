using System.Collections.Generic;

namespace Lovensedotnet.Models
{
    public class User
    {
        public string Name { get; set; }
        public Dictionary<string, Toy> Toys { get; set; }
        public LovenseApp Mode { get; set; }
        public string RequestURL
        {
            get
            {
                if (Mode == LovenseApp.Callback)
                {
                    return RequestSAPI;
                }
                else
                {
                    return RequestLAN;
                }
            }
        }
        public string RequestSAPI { get; set; }
        public string RequestLAN { get; set; }
    }
}
