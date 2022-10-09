using Data;
using LovenseService.Validators;
using System;
using System.Text;

namespace Data.Builders
{
    public class PatternBuilder
    {
        private StringBuilder Structure = new("V:1;");
        private LovenseAction[] Actions;
        private int Interval;
        private StringBuilder Pattern = new();

        public string GetPattern()
        {
            StringBuilder functions = new();
            foreach(var action in Actions) { functions.Append(action.ToString().Substring(0, 1).ToLower()); }
            Structure.Append($"F:{functions};");
            Structure.Append($"S:{Interval}#");
            return Structure.ToString();
        }

        public string GetStructure()
        {
            return Pattern.ToString()[0..^1];
        }
        public void DefineStructure(LovenseAction[] actions, int MillisecInterval)
        {
            Actions = actions;
            Interval = MillisecInterval;
        }

        public void Append(int actionStrength)
        {
            Array.ForEach(Actions, action => CommandStrengthValidator.Validate(actionStrength, action));
            Pattern.Append($"{actionStrength};");
        }
    }
}