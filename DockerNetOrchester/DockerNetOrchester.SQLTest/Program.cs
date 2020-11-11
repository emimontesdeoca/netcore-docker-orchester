using DockerNetOrchester.Data;
using System;
using System.Linq;
using System.Xml.Schema;

namespace DockerNetOrchester.SQLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = args.Length > 0 ? args[0] : string.Empty;

            if (String.IsNullOrEmpty(cs))
            {
                Log($"Argument is empty, pass a connection string to test");
            }
            else
            {
                Log($"Argument introduced: '{cs}'");

                Model.CS = cs;

                var ctx = new Model();
                var jobs = ctx.Jobs.ToList();

                Log($"Jobs found: '{jobs.Count()}'");
            }
        }

        static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] - {message}");
        }
    }
}
