using System.Collections.Generic;

namespace LovenseData.Models
{
    public class User
    {
        public string Name { get; set; }
        public List<Toy> Toys { get; set; }
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
        public static string RequestSAPI { get; set; }
        public string RequestLAN { get; set; }
    }
}
