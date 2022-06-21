using Lovensedotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Lovensedotnet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly LovenseClient client;
        private IConfiguration configuration;
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
        [HttpPost("gm/SetPort/{port}")]
        public async Task<IActionResult> SetPort(string port)
        {
            configuration["GMPort"] = port;
            return base.Ok();
        }
        [HttpPost("Toys/{index}/Ping")]
        public async Task<IActionResult> SinglePing(int index)
        {
            Toy toy = await client.GetToyAtIndex(index);
            await client.Ping(toy);
            return base.Ok();
        }
        [HttpPost("Toys/{index}/ContinuousPing")]
        public async Task<IActionResult> SinglePing(int index, int interval = 1)
        {
            Toy toy = await client.GetToyAtIndex(index);
            client.ContinuousPing(toy, interval, Global.PingCancellationTokens.Token);
            return base.Ok();
        }
        [HttpPost("Toys/CancelActivePings")]
        public async Task<IActionResult> CancelActivePings()
        {
            Global.PingCancellationTokens.Cancel();
            return base.Ok();
        }
    }
}
