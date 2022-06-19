using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Driver.DTO;
using Driver.Exceptions;
using Newtonsoft.Json;

namespace Driver
{
    public class Driver
    {
        private HttpClient Client = new();
        private string DevToken = "vRpBTU_hTDb3np6L8Ve0YQL_CSzVoOwIda0BxlTeY7GBmHdMRkXPeBOxb6Xk11Zk";
        private string DevID = "Woonkamerplant";

        public async Task<string> GetQR()
        {

            var response = await Client.PostAsJsonAsync("https://api.lovense.com/api/lan/getQrCode", new AuthDTO() { Token = DevToken, UID = DevID });
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var QR = JsonConvert.DeserializeObject<QRDTO>(await response.Content.ReadAsStringAsync());
                return QR.Message;
            }
            else
            {
                throw new NoReturnUrlException();
            }
        }
    }
}
