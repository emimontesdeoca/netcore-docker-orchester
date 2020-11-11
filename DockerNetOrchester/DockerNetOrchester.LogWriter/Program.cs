using System;
using System.Collections.Generic;

namespace DockerNetOrchester.LogWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            var thingToWrite = args.Length > 0 ? args[0] : DateTime.UtcNow.ToString();
            //System.IO.File.AppendAllLines("log.txt", new List<string>() { thingToWrite });
            Console.WriteLine($"Hello World: {thingToWrite}");
        }
    }
}
