using DockerNetOrchester.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;

namespace DockerNetOrchester.Orchester
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", true, true)
              .Build();

            Log($"Starting orchester");

            // Create bunch of jobs
            var jobList = new List<Job>();

            var nJobs = Int32.Parse(configuration["Configurations:JobsAmount"]);
            var nAgents = Int32.Parse(configuration["Configurations:AgentsAmount"]);

            for (int i = 0; i < nJobs; i++)
            {
                var newJob = new Job()
                {
                    Id = Guid.NewGuid(),
                    Done = false
                };

                jobList.Add(newJob);
            }

            Log($"Created {nJobs} jobs");

            // Splitting list
            var nList = nJobs / nAgents;

            var splittedList = SplitList(jobList, nList);
            Log($"Splittin in list of {nList}");

            Log($"Creating AgentJobs for each splitted list");
            var agentJobList = new List<AgentJob>();

            var agentNames = new List<string>();

            var agentNumber = 1;
            foreach (var list in splittedList)
            {
                var agentName = $"Agent{agentNumber}";

                Log($"Creating agentJob for {agentName}");

                foreach (var item in list)
                {
                    var newAgent = new AgentJob()
                    {
                        Id = Guid.NewGuid(),
                        AgentName = $"Agent{agentNumber}",
                        JobId = item.Id
                    };

                    agentJobList.Add(newAgent);
                }

                Log($"Finished creating agentJob for {agentName}");

                agentNames.Add(agentName);
                agentNumber++;
            }

            var linuxConnectionString = configuration["ConnectionStrings:LinuxHostConnectionString"];
            Model.CS = linuxConnectionString;

            var ctx = new Model();

            Log($"Adding jobs to database");
            ctx.Jobs.AddRange(jobList);

            Log($"Adding agentJobs to database");
            ctx.AgentJobs.AddRange(agentJobList);

            ctx.SaveChanges();


            // Run bash commands to generate the jobs
            var containerConnectionString = configuration["ConnectionStrings:ContainerConnectionString"];

            foreach (var agentName in agentNames)
            {
                Log($"Starting agent: ''");
                var bash = $"docker run --rm dockernetorchesteragent \"{containerConnectionString}\" \"{agentName}\"";
                Bash(bash);
            }
        }

        static void Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
        }


        static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] - {message}");
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}
