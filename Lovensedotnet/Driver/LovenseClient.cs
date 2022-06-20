using ApiClient.DTO;
using Driver.DTO;
using Driver.Exceptions;
using Driver.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Driver
{
    public class LovenseClient
    {
        private readonly HttpClient Client = new();
        public string BaseURL { get; set; }
        public string QRURL { get; set; } = "https://api.lovense.com/api/lan/getQrCode";
        public string DevToken { get; set; }
        public string DevID { get; set; }
        private CallbackRequest callback;
        public CallbackRequest Callback 
        {
            get { return callback; } 
            set
            {
                Toys = value.Toys;
                callback = value;
            } 
        }
        public Dictionary<string, Toy> Toys { get; set; }
        public LovenseClient(string token, string devId)
        {
            DevToken = token;
            DevID = devId;
        }
        public async Task<string> GetQR()
        {
            var response = await Client.PostAsJsonAsync
                ( QRURL,
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
            // Check if the callback is null to differentiate between GameMode
            // Which doesn't need token & uid
            // And Standard API, which does.

            if(callback != null)
            {
                command.Token = DevToken;
                command.UserID = Callback.Uid;
            }

            // Checks if the ToyID can be parsed. If this is the case, it's an index meant to grab the actual ID
            // If the ID is an empty string, it's meant to target all toys.
            // if the ID is an unparsable string, it's most likely already a valid ID.

            if (command.TargetToyID != "" && int.TryParse(command.TargetToyID, out int index))
            {
                command.TargetToyID = GetToyAtIndex(index).Result.ID;
            }
            await Client.PostAsJsonAsync(
                BaseURL + "/command",
                command
                );
        }
        public async Task<Toy> GetToyAtIndex(int index)
        {
            return Toys.ElementAt(index).Value;
        }
        public async Task<Dictionary<string, Toy>> GetToys()
        {
            var response =  Client.GetAsync($"{BaseURL}/GetToys").Result;
            var dto = JsonConvert.DeserializeObject< GetToysDTO>(await response.Content.ReadAsStringAsync());
            return Toys = dto.Toys;
        }
    }
}
