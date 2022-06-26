using LovenseData;
using LovenseData.Interfaces;
using LovenseData.Models;
using LovenseService.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LovenseService
{
    public class LovenseClient
    {
        private readonly HttpClient Client = new();
        public string QRURL { get; set; } = "https://api.lovense.com/api/lan/getQrCode";
        public string Token { get; set; }
        public int UserCount { get; set; }

        private readonly IUserRepository Users;
        private readonly IToyRepository Toys;
        private readonly IConfiguration Configuration;

        public LovenseClient(IUserRepository users, IToyRepository toys, IConfiguration configuration)
        {
            Users = users;
            Toys = toys;
            Configuration = configuration;
            Token = Configuration["DevToken"];
        }
        private async Task<string> Push<T>(T dto, string url)
        {
            string json = JsonConvert.SerializeObject(dto);
            HttpContent content = new StringContent(json , Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(url, content).Result.Content.ReadAsStringAsync();
            Debug.WriteLine(json);
            Debug.WriteLine(response);
            return response;
        }

        public async Task<QRDTO> GetQRv2(string user)
        {
            var dto = new AuthDTO()
            {
                DevToken = Token,
                UserID = user,
                ApiVer = 2
            };
            return JsonConvert.DeserializeObject<QRDTO>(await Push(dto, QRURL));
        }

        public void AddUser(User user)
        {
            if (!Users.CheckForUser(user.Name))
            {
                Users.Add(user);
                foreach (Toy toy in user.Toys)
                {
                    toy.Owner = user.Name;
                    Toys.Add(toy);
                }
            }
            else if (user.Mode == LovenseApp.Callback)
            {
                Users.Update(user);
            };
        }

        public async Task DoCMD(CommandDTO command)
        {
            command.Token = Token;


            if (command.TargetToyID != "" && int.TryParse(command.TargetToyID, out int index))
            {
                command.TargetToyID = GetToyAtIndex(index).Result.ID;
                var user = Users.GetUserByID(
                    Toys.GetToyByID(command.TargetToyID).Owner);
                command.Username = user.Name;
                await Push(command,
                user.RequestURL + "/command"
                );
            }
            else
            {
                command.Username = Users.ToString();
                await Push(command,
                User.RequestSAPI + "/command"
                );
            }
        }

        public async Task<List<Toy>> GetToys(User? user = null)
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
                            Toys.Add(keyValuePair.Value);

                        }
                        return ctdto.Toys.Values.ToList();
                    case LovenseApp.Remote:
                        var rtdto = JsonConvert.DeserializeObject<RemoteToysDTO>(await Push
                            (new CommandDTO() { Command = LovenseCommand.GetToys },
                                $"{user.RequestURL}/command"));
                        foreach (var keyValuePair in rtdto.Data.Toys)
                        {
                            Toys.Add(keyValuePair.Value);
                        }
                        return rtdto.Data.Toys.Values.ToList();
                    case LovenseApp.Callback:
                        return user.Toys;
                    default:
                        return Toys.GetAll();
                }
            }
            else
            {
                return Toys.GetAll();
            }
        }

        public List<Toy> GetToysByUserId(string userId)
        {
            return Users.GetUserByID(userId).Toys;
        }

        public async Task Prune()
        {
            foreach (var toy in Toys.GetAll())
            {
                try
                {
                    await Ping(toy);
                }
                catch
                {
                    Toys.Remove(toy);
                    Users.GetUserByID(toy.Owner).Toys.Remove(toy);
                }
            }
        }

        public void SetUserMode(string userId, LovenseApp mode)
        {
            User user = Users.GetUserByID(userId);
            user.Mode = mode;
            Users.Update(user);
        }

        public Task<Toy> GetToyAtIndex(int index)
        {
            return Task.FromResult(Toys.GetToyAtIndex(index));
        }
        public async Task Ping(Toy? toy)
        {
            var dto = new CommandDTO()
            {
                Token = Token,
                Username = toy.Owner,
                TargetToyID = toy.ID,
                Command = LovenseCommand.Function,
                Action = "Vibrate:5",
                Duration = 0.5,
            };
            await Push(dto, Users.GetUserByID(toy.Owner).RequestURL + "/command");
            
        }

        public async Task ContinuousPing(Toy toy, int interval, CancellationToken t)
        {
            while (!t.IsCancellationRequested)
            {
                await Push(
                 new CommandDTO()
                 {
                     Token = Token,
                     Username = toy.Owner,
                     Command = LovenseCommand.Function,
                     Action = "Vibrate:5",
                     Duration = 1,
                 },
                 Users.GetUserByID(toy.Owner).RequestURL + "/command");
                await Task.Delay(interval * 1000, t);
            }
        }
    }
}
