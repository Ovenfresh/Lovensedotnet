using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Exceptions
{
    public class NoReturnUrlException : Exception
    {
        public NoReturnUrlException() : base( message: "The URL Request did not return a url as expected.")
        {

        }
    }
}
