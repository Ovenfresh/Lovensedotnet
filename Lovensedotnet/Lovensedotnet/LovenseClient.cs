using Lovensedotnet.DTO;
using Lovensedotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Lovensedotnet
{
    public class LovenseClient
    {
        private readonly HttpClient Client = new();
        public string QRURL { get; set; } = "https://api.lovense.com/api/lan/getQrCode";
        public string DevToken { get; set; }
        public string DevID { get; set; }
        public Dictionary<string, User> Users { get; set; } = new();
        public Dictionary<string, Toy> Toys { get; set; } = new();
        public LovenseClient(string token, string devId)
        {
            DevToken = token;
            DevID = devId;
        }
        public async Task<QRDTO> GetQRv1(string user)
        {
            var response = await Client.PostAsJsonAsync
                (QRURL,
                new AuthDTO() { DevToken = DevToken, UserID = user });

            return JsonConvert.DeserializeObject<QRDTO>(await response.Content.ReadAsStringAsync());

        }
        public async Task<QRDTO> GetQRv2(string user)
        {
            var response = await Client.PostAsJsonAsync
                (QRURL,
                new AuthDTO() { DevToken = DevToken, UserID = user, Version = 2 });

            return JsonConvert.DeserializeObject<QRDTO>(await response.Content.ReadAsStringAsync());

        }
        public async Task DoCMD(CommandDTO command)
        {
            command.Token = DevToken;
            Users.TryGetValue(command.UserID, out User user);

            if (command.TargetToyID != "" && int.TryParse(command.TargetToyID, out int index))
            {
                command.TargetToyID = GetToyAtIndex(index).Result.ID;
            }
            await Client.PostAsJsonAsync(
                user.RequestURL + "/command",
                command
                );
        }
        public async Task<Dictionary<string, Toy>> GetToys(User? user = null)
        {
            HttpResponseMessage response;

            // Lovense Connect & Lovense Remote each deliver differenctly structured JSON, so there must be a differentiation between the two. 
            // Option "Callback" for when the data was received through callback

            if (user is not null)
            {
                switch (user.Mode)
                {
                    case LovenseApp.Connect:
                        response = Client.GetAsync($"{user.RequestURL}/GetToys").Result;
                        var ctdto = JsonConvert.DeserializeObject<ConnectToysDTO>(await response.Content.ReadAsStringAsync());
                        foreach (var keyValuePair in ctdto.Toys)
                        {
                            if (!Toys.ContainsKey(keyValuePair.Key))
                            {
                                keyValuePair.Value.Owner = user;
                                Toys.Add(user.Name, keyValuePair.Value);
                            }

                        }
                        return ctdto.Toys;
                    case LovenseApp.Remote:
                        response = Client.PostAsJsonAsync($"{user.RequestURL}/command", new CommandDTO() { Command = "GetToys" }).Result;
                        var rtdto = JsonConvert.DeserializeObject<RemoteToysDTO>(response.Content.ReadAsStringAsync().Result);
                        foreach (var keyValuePair in rtdto.Data.Toys)
                        {
                            if (!Toys.ContainsKey(keyValuePair.Key))
                            {
                                keyValuePair.Value.Owner = user;
                                Toys.Add(user.Name, keyValuePair.Value);
                            }
                        }
                        return rtdto.Data.Toys;
                    case LovenseApp.Callback:
                        return user.Toys;
                    default:
                        return Toys;
                }
            }
            else
            {
                return Toys;
            }
        }
        public Task<Toy> GetToyAtIndex(int index)
        {
            return Task.FromResult(Toys.ElementAt(index).Value);
        }
        public async Task Ping(Toy toy)
        {
            await Client.PostAsJsonAsync(toy.Owner.RequestURL + "/command",
                 new CommandDTO()
                 {
                     Token = DevToken,
                     UserID = toy.Owner.Name,
                     Command = "Function",
                     Action = "Vibrate:5",
                     Duration = 1,
                 });
        }

        public async Task ContinuousPing(Toy toy, int interval, CancellationToken t)
        {
            while (!t.IsCancellationRequested)
            {
                await Client.PostAsJsonAsync(toy.Owner.RequestURL + "/command",
                 new CommandDTO()
                 {
                     Token = DevToken,
                     UserID = toy.Owner.Name,
                     Command = "Function",
                     Action = "Vibrate:5",
                     Duration = 1,
                 });
                await Task.Delay(interval * 1000, t);
            }
        }
    }
}
