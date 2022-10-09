using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovenseService.Validators
{
    public static class CommandStrengthValidator
    {
        public class StrengthOutOfRangeException : Exception
        {
            public StrengthOutOfRangeException(int lower, int upper)
                : base($"Strength must be between {lower} & {upper}")
            {
            }
        }
        public static bool Validate(int strength, LovenseAction action)
        {
            int upperLimit = (action == LovenseAction.Pump) ? 3 : 20; 
            if (strength < 0 || strength > upperLimit)
            {
                throw new StrengthOutOfRangeException(0, upperLimit);
            }
            else
            {
                return true;
            }
        }
    }
}
