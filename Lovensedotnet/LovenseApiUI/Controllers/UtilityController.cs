using LovenseData;
using LovenseData.Models;
using LovenseService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LovenseApiUI.Controllers
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
        [HttpPost("SetToken")]
        public async Task<IActionResult> SetToken(string DevToken)
        {
            client.Token = DevToken;
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
                Toys = response.Toys.Values.ToList(),
                RequestLAN = $"https://{response.Domain}:{response.HttpsPort}"
            };
            client.AddUser(user);
            return base.Ok($"Added user {response.Uid}");
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
                Name = alias ?? $"Device {client.UserCount + 1}",
                Mode = Enum.Parse<LovenseApp>(app, true),
                RequestLAN = $"https://{ip.Replace(".", "-")}.{configuration["BaseGM"]}:{port}"
            };
            Debug.WriteLine(user.RequestURL);
            user.Toys = await client.GetToys(user);
            client.AddUser(user);
            return base.Ok(user);
        }
        [HttpPost("Users/{userId}/{mode}")]
        public async Task<IActionResult> SetUserMode(string userId, LovenseApp mode)
        {
            try
            {
                client.SetUserMode(userId, mode);
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
            await client.Prune();
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
            var toys = client.GetToysByUserId(userId);
            if (toys is not null)
            {
                return base.Ok(toys);
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
