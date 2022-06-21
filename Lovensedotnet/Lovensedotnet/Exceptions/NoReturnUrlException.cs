using System;

namespace Lovensedotnet.Exceptions
{
    public class NoReturnUrlException : Exception
    {
        public NoReturnUrlException() : base(message: "The URL Request did not return a url as expected.")
        {

        }
    }
}
