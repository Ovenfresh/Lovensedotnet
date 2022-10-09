using Data;
using Data.DTO;
using Data.Interfaces;
using LovenseService.DTO;
using LovenseService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Builders
{
    public  class FunctionRequestBuilder : IFluentRequestBuilder<FunctionRequestBuilder>
    {
        public CommandDTO Request { get; set; } = new();
        private static string _DeveloperToken;
        public  string DeveloperToken { set => _DeveloperToken = value; }

        public  FunctionRequestBuilder SetAction(LovenseAction action, int strength)
        {
            if (CommandStrengthValidator.Validate(strength, action))
            {
                Request.Action = $"{action}:{strength}";
            }
            return this;
        }

        public  FunctionRequestBuilder SetLoopLength(int loopRunningSec)
        {
            Request.LoopLength = loopRunningSec;
            return this;
        }

        public  FunctionRequestBuilder SetLoopInterval( int loopPauseSec)
        {
            Request.LoopInterval = loopPauseSec;
            return this;
        }

        public FunctionRequestBuilder SetToyOwner(string id)
        {
            Request.Username = id;
            return this;
        }

        public FunctionRequestBuilder SetDeveloperToken(string token)
        {
            DeveloperToken = token;
            return this;
        }

        public FunctionRequestBuilder SetCommandType(LovenseCommand command)
        {
            Request.Command = command;
            return this;
        }

        public FunctionRequestBuilder SetRuntime(int timeSec)
        {
            Request.Duration = timeSec;
            return this;
        }

        public FunctionRequestBuilder SetTargetToyID(string toyID)
        {
            Request.TargetToyID = toyID;
            return this;
        }

        public CommandDTO Build()
        {
            Request.Token = _DeveloperToken;
            return Request;
        }
    }
}
