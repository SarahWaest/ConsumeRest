using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ModelLib.Model;
using Newtonsoft.Json;

namespace ConsumeRest
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker w1 = new Worker();

            w1.Start();
            Console.ReadLine();
        }
    }

}
