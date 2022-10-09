using Data.DTO;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Builders
{
    public  class PresetRequestBuilder : IFluentRequestBuilder<PresetRequestBuilder>
    {
        public CommandDTO Request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DeveloperToken { set => throw new NotImplementedException(); }

        public CommandDTO Build()
        {
            throw new NotImplementedException();
        }

        public PresetRequestBuilder SetCommandType(LovenseCommand command)
        {
            throw new NotImplementedException();
        }

        public PresetRequestBuilder SetDeveloperToken(string token)
        {
            throw new NotImplementedException();
        }

        public PresetRequestBuilder SetPreset(string name)
        {
            Request.PresetName = name;
            return this;
        }

        public PresetRequestBuilder SetRuntime(int timeSec)
        {
            throw new NotImplementedException();
        }

        public PresetRequestBuilder SetTargetToyID(string toyID)
        {
            throw new NotImplementedException();
        }

        public PresetRequestBuilder SetToyOwner(string id)
        {
            throw new NotImplementedException();
        }
    }
}
