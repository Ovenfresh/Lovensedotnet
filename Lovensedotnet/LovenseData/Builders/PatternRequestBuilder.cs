using Data;
using Data.Interfaces;
using Data.DTO;
using LovenseService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Builders
{
    public  class PatternRequestBuilder : IFluentRequestBuilder<PatternRequestBuilder>
    {
        private static PatternBuilder patternBuilder = new();
        private static string _DeveloperToken;

        public CommandDTO Request { get; set; } = new();
        public string DeveloperToken { set => _DeveloperToken = value; }

        public PatternRequestBuilder DefineStructure(LovenseAction[] actions, int millisecInterval)
        {
            patternBuilder.DefineStructure(actions, millisecInterval);
            return this;
        }

        public PatternRequestBuilder AddPatternStep(int actionStrength) 
        {
            patternBuilder.Append(actionStrength);
            return this;
        }

        public  CommandDTO Build()
        {
            Request.Structure = patternBuilder.GetPattern();
            Request.Sequence = patternBuilder.GetStructure();
            Request.Token = _DeveloperToken;
            return Request;
        }

        public PatternRequestBuilder SetToyOwner(string id)
        {
            Request.Username = id;
            return this;
        }

        public PatternRequestBuilder SetDeveloperToken(string token)
        {
            DeveloperToken = token;
            return this;
        }

        public PatternRequestBuilder SetCommandType(LovenseCommand command)
        {
            Request.Command = command;
            return this;
        }

        public PatternRequestBuilder SetRuntime(int timeSec)
        {
            Request.Duration = timeSec;
            return this;
        }

        public PatternRequestBuilder SetTargetToyID(string toyID)
        {
            Request.TargetToyID = toyID;
            return this;
        }
    }
}
