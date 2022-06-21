using Lovensedotnet.DTO;
using Lovensedotnet.Exceptions;
using Lovensedotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Lovensedotnet
{
    public class LovenseClient
    {
        private readonly HttpClient Client = new();
        public string BaseURL { get; set; }
        public string QRURL { get; set; } = "https://api.lovense.com/api/lan/getQrCode";
        public string DevToken { get; set; }
        public string DevID { get; set; }
        private CallbackRequest callback;

        // Once the callback is received, the toys are extracted & have an owner assigned to them.
        public CallbackRequest Callback
        {
            get { return callback; }
            set
            {
                Toys = value.Toys;
                foreach (var keyValuePair in Toys) { keyValuePair.Value.Owner = value.Uid; }
                callback = value;
            }
        }
        public Dictionary<string, Toy> Toys { get; set; } = new();
        public LovenseClient(string token, string devId)
        {
            DevToken = token;
            DevID = devId;
        }
        public async Task<string> GetQR()
        {
            var response = await Client.PostAsJsonAsync
                (QRURL,
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

            if (callback != null)
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
        public async Task<Dictionary<string, Toy>> GetToys(LovenseApp app)
        {
            HttpResponseMessage response;

            // Lovense Connect & Lovense Remote each deliver differenctly structured JSON, so there must be a differentiation between the two. 
            // Option "Callback" for when the data was received through callback

            switch (app)
            {
                case LovenseApp.Connect:
                    response = Client.GetAsync($"{BaseURL}/GetToys").Result;
                    var ctdto = JsonConvert.DeserializeObject<ConnectToysDTO>(await response.Content.ReadAsStringAsync());
                    foreach (var keyValuePair in ctdto.Toys)
                    {
                        if (!Toys.ContainsKey(keyValuePair.Key))
                        {
                            Toys.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }
                    break;
                case LovenseApp.Remote:
                    response = Client.PostAsJsonAsync($"{BaseURL}/command", new CommandDTO() { Command = "GetToys" }).Result;
                    var rtdto = JsonConvert.DeserializeObject<RemoteToysDTO>(response.Content.ReadAsStringAsync().Result);
                    foreach (var keyValuePair in rtdto.Data.Toys)
                    {
                        if (!Toys.ContainsKey(keyValuePair.Key))
                        {
                            Toys.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }
                    break;
                case LovenseApp.Callback:
                    break;
            }
            return Toys;
        }
        public Task<Toy> GetToyAtIndex(int index)
        {
            return Task.FromResult(Toys.ElementAt(index).Value);
        }
        public async Task Ping(Toy toy)
        {
            await Client.PostAsJsonAsync(BaseURL + "/command",
                 new CommandDTO()
                 {
                     Token = DevToken,
                     UserID = toy.Owner,
                     Command = "Function",
                     Action = "Vibrate:5",
                     Duration = 1,
                 });
        }

        public async Task ContinuousPing(Toy toy, int interval, CancellationToken t)
        {
            while (!t.IsCancellationRequested)
            {
                await Client.PostAsJsonAsync(BaseURL + "/command",
                 new CommandDTO()
                 {
                     Token = DevToken,
                     UserID = toy.Owner,
                     Command = "Function",
                     Action = "Vibrate:5",
                     Duration = 1,
                 });
                await Task.Delay(interval * 1000, t);
            }
        }
    }
}
