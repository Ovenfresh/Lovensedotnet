using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Driver.DTO
{
    public class CommandDTO
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("uid")]
        public string UserID { get; set; }
        [Required]
        [JsonProperty("command")]
        public string Command { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("name")]
        public string PresetName { get; set; }
        [JsonProperty("rule")]
        public string Pattern { get; set; }
        public string Strength { get; set; }
        [Required]
        [JsonProperty("timeSec")]
        public int Duration { get;set; }
        [JsonProperty("loopRunningSec")]
        public int LoopLength { get; set; }
        [JsonProperty("loopPauseSec")]
        public int LoopInterval { get; set; }
        [JsonProperty("toy")]
        public string TargetToyID { get; set; }
        [Required]
        [JsonProperty("apiVer")]
        public readonly int ApiVersion = 1;
    }
}
