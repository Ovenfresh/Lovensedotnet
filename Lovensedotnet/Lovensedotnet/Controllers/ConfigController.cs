using Lovensedotnet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly LovenseClient client;
        public ConfigController(LovenseClient client)
        {
            this.client = client;
        }
        [HttpPost("Dev")]
        public async Task<IActionResult> SetDevCredentials(string DevID, string DevToken)
        {
            client.DevToken = DevToken;
            client.DevID = DevID;
            return base.Ok();
        }
    }
}
