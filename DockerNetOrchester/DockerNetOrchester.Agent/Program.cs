using DockerNetOrchester.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace DockerNetOrchester.Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            var agentName = args.Length > 0 ? args[1] : string.Empty;
            var connectionString = args.Length > 0 ? args[0] : string.Empty;

            if (String.IsNullOrEmpty(agentName) || String.IsNullOrEmpty(connectionString))
            {
                Log("Agent or connection string not set, exiting");
            }
            else
            {
                // Log agent
                Log($"Starting agent: {agentName}");

                Model.CS = connectionString;

                // Set context
                var ctx = new Model();

                // Get from queue where the assigned AgentName is same as mine
                var jobsForMe = ctx.AgentJobs.Where(x => x.AgentName == agentName).ToList();
                Log($"Found {jobsForMe.Count()} jobs for me");

                // List to keep track of finished jobs
                var listFinishedJobs = new List<Guid>();

                // Do some stuff
                foreach (var item in jobsForMe)
                {
                    Log($"Starting job with id: {item.Id}");
                    var job = ctx.Jobs.Single(x => x.Id == item.JobId);

                    // random timeout
                    Random rnd = new Random();
                    int number = rnd.Next(1, 5);

                    Thread.Sleep(number * 1000);

                    job.Done = true;

                    listFinishedJobs.Add(item.Id);
                    Log($"Finished job with id: {item.Id}");

                    // Done
                    ctx.SaveChanges();
                }

                // Cleanup
                var toRemoveAgentJobs = ctx.AgentJobs.Where(x => listFinishedJobs.Contains(x.Id)).ToList();
                ctx.AgentJobs.RemoveRange(toRemoveAgentJobs);
                Log($"Cleaned up {toRemoveAgentJobs.Count()} jobs from queue since they are finished already");

                // Done
                ctx.SaveChanges();
                Log($"Saved to database");
            }
        }

        static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] - {message}");
        }
    }
}
