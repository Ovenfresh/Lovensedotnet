using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Driver;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            EroDriver client = new();
            client.GetQR();
            client.GetToys();
        }
    }
}
