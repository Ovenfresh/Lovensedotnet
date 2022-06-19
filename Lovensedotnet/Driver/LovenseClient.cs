using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Driver.DTO;
using Driver.Exceptions;
using Newtonsoft.Json;
using System.Diagnostics;
using Driver.Models;
using System.Collections.Generic;
using System.Linq;

namespace Driver
{
    public class LovenseClient
    {
        private HttpClient Client = new();
        private readonly string DevToken;
        private readonly string DevID;
        public CallbackRequest Callback { get; set; }
        public string UserID { get; set; }
        public LovenseClient(string token, string devId)
        {
            DevToken = token;
            DevID = devId;
        }
        public async Task<string> GetQR()
        {
            var response = await Client.PostAsJsonAsync
                ("https://api.lovense.com/api/lan/getQrCode", 
                new AuthDTO() { Token = DevToken, UID = DevID });
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var QR = JsonConvert.DeserializeObject<QRDTO>(await response.Content.ReadAsStringAsync());
                return QR.URL;
            }
            else
            {
                throw new NoReturnUrlException();
            }
        }
        public async Task DoCMD(CommandDTO command)
        {
            command.Token = DevToken;
            command.UserID = Callback.Uid;

            var response = await Client.PostAsJsonAsync(
                "https://api.lovense.com/api/lan/v2/command",
                command
                );
        }
        public async Task<Toy> GetToyAtIndex(int index)
        {
            return Callback.Toys.ElementAt(index).Value;
        }
    }
}
