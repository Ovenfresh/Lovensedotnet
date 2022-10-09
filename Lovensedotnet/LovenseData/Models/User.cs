using System.Collections.Generic;

namespace Data.Models
{
    public class Owner
    {
        public string Name { get; set; }
        public List<Toy> Toys { get; set; }
        public LovenseApp Mode { get; set; }
        public string ApiRequestURL
        {
            get
            {
                if (Mode == LovenseApp.Callback)
                {
                    return StandardApiURL;
                }
                else
                {
                    return LanApiURL;
                }
            }
        }
        public static string StandardApiURL { get; set; }
        public string LanApiURL { get; set; }
    }
}
