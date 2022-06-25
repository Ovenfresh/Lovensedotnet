using Lovensedotnet.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Lovensedotnet.Controllers
{
    [Route("Lovensedotnet/sapi")]
    [ApiController]
    public class StandardApiController : ControllerBase
    {
        private LovenseClient client;
        private IConfiguration configuration;
        public StandardApiController(LovenseClient driver, IConfiguration configuration)
        {
            this.client = driver;
            this.configuration = configuration;
        }

        [HttpGet("Toys/{index}")]
        [SwaggerOperation("Index starts at 0, returns entry from the Toys-dictionary in the Callback.")]
        public async Task<IActionResult> GetToyAtIndex(int index)
        {
            return base.Ok(client.GetToyAtIndex(index));
        }
        [HttpPost("Toys/Vibrate")]
        [SwaggerOperation("Intensity scales from 0 - 20 | Duration, Length & Interval are in seconds.")]
        public async Task<IActionResult> Vibrate
            (int intensity = 8, int duration = 10, int loopLength = 0, int loopInterval = 0, string toyId = "")
        {
            await client.DoCMD(new CommandDTO()
            {
                Command = "Function",
                Action = $"Vibrate:{intensity}",
                Duration = duration,
                LoopLength = loopLength,
                LoopInterval = loopInterval,
                TargetToyID = toyId
            });
            return base.Ok();
        }
        [HttpPost("Toys/Preset")]
        [SwaggerOperation("Plays predefined preset based on the name, duration in seconds.")]
        public async Task<IActionResult> PlayPreset(string presetName, int duration = 10, string toyId = "")
        {
            await client.DoCMD(new CommandDTO()
            {
                Command = "Preset",
                PresetName = presetName,
                Duration = duration,
                TargetToyID = toyId
            });
            return base.Ok();
        }

        [HttpPost("Toys/Pattern")]
        [SwaggerOperation("Strength scales from 0 - 20 and is seperated by ';' | Duration(s) | Interval (ms) determines how quickly the loop cycles " +
            "through the strengths for the length of the duration")]
        public async Task<IActionResult> PlayPattern(string functions, string strength, string interval, int duration = 10, string toyId = "")
        {
            await client.DoCMD(new CommandDTO()
            {
                Command = "Pattern",
                Structure = $"V:1;F:{functions};S:{interval}#",
                Pattern = strength,
                Duration = duration,
                TargetToyID = toyId
            });
            return base.Ok();
        }
    }
}
