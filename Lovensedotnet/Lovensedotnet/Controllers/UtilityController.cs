using Lovensedotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lovensedotnet.Controllers
{
    [Route("Lovensedotnet/utl")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly LovenseClient client;
        private readonly IConfiguration configuration;
        public UtilityController(LovenseClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }
        [HttpPost("SetDevCredentials")]
        public async Task<IActionResult> SetDevCredentials(string DevID, string DevToken)
        {
            client.DevToken = DevToken;
            client.DevID = DevID;
            return base.Ok();
        }

        #region Callback Handling & QR generation
        [HttpPost("Callback")]
        [SwaggerOperation("This POST operation exists solely to receive the callback from the lovense servers after the QR has been scanned.")]
        public async Task<IActionResult> CatchResponse([FromBody] CallbackRequest response)
        {
            User user = new()
            {
                Mode = LovenseApp.Callback,
                Name = response.Uid,
                Toys = response.Toys,
                RequestSAPI = configuration["BaseSAPI"],
                RequestLAN = $"https://{response.Domain}:{response.HttpsPort}"
            };

            if (!client.Users.ContainsKey(response.Uid))
            {
                client.Users.Add(response.Uid, user);
                foreach (var pair in user.Toys)
                {
                    pair.Value.Owner = user;
                    client.Toys.Add(pair.Key, pair.Value);
                }
                return base.Ok($"Added user {response.Uid}");
            }
            else
            {
                if (user.Mode == LovenseApp.Callback)
                {
                    client.Users[response.Uid] = user;
                    foreach (var pair in user.Toys)
                    {
                        pair.Value.Owner = user;
                        client.Toys[pair.Key] = pair.Value;
                    }
                    return base.Ok($"Updated user {response.Uid}");
                }
                return base.Ok();
            }
        }

        [HttpGet("QR/v1/{username}")]
        public async Task<IActionResult> GetQRv1(string username)
        {
            return base.Ok(await client.GetQRv1(username));
        }
        [HttpGet("QR/v2/{username}")]
        public async Task<IActionResult> GetQRv2(string username)
        {
            return base.Ok(await client.GetQRv2(username));
        }
        #endregion

        [HttpPost("Users/Add")]
        public async Task<IActionResult> AddUser(string app, string? alias, string ip = "192.168.0.206", int port = 30013)
        {
            User user = new()
            {
                Name = (alias == null) ? $"Device {client.Users.Count + 1}" : alias,
                Mode = Enum.Parse<LovenseApp>(app, true),
                RequestLAN = $"https://{ip.Replace(".", "-")}.{configuration["BaseGM"]}:{port}"
            };
            Debug.WriteLine(user.RequestURL);
            user.Toys = await client.GetToys(user);
            client.Users.Add(user.Name, user);

            return base.Ok(user);
        }
        [HttpPost("Users/{userId}/{mode}")]
        public async Task<IActionResult> SetUserMode(string userId, LovenseApp mode)
        {
            try
            {
                client.Users[userId].Mode = mode;
                return base.Ok($"Set mode for {userId} to {mode} ");
            }
            catch
            {
                return base.Problem($"Userid {userId} not found in the dictionary.");
            }
        }
        [HttpGet("Users/Prune")]
        public async Task<IActionResult> PruneUsers()
        {
            foreach (var UserDefiniton in client.Users)
            {
                try
                {
                    await client.Ping(UserDefiniton.Value.Toys.ElementAt(0).Value);
                }
                catch
                {
                    client.Users.Remove(UserDefiniton.Key);
                }
            }
            return base.Ok();
        }
        [HttpGet("GetToys")]
        public async Task<IActionResult> GetToys()
        {
            return base.Ok(await client.GetToys());
        }
        [HttpGet("GetToys/{userId}")]
        public async Task<IActionResult> GetToysByUser(string userId)
        {
            if (client.Users.ContainsKey(userId))
            {
                var user = client.Users[userId];
                return base.Ok(user.Toys);
            }
            else
            {
                return base.NoContent();
            }
        }
        [HttpPost("Toys/{index}/Ping")]
        public async Task<IActionResult> SinglePing(int index)
        {
            Toy toy = await client.GetToyAtIndex(index);
            await client.Ping(toy);
            return base.Ok();
        }
        [HttpPost("Toys/{index}/ContinuousPing")]
        public async Task<IActionResult> ContinuousPing(int index, int interval = 1)
        {
            Toy toy = await client.GetToyAtIndex(index);
            client.ContinuousPing(toy, interval, Global.PingCancellationTokens.Token);
            return base.Ok();
        }
        [HttpPost("Toys/CancelActivePings")]
        public async Task<IActionResult> CancelActivePings()
        {
            Global.PingCancellationTokens.Cancel();
            Global.PingCancellationTokens = new();
            return base.Ok();
        }
    }
}
