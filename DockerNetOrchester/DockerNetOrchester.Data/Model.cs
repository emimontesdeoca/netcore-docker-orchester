using Microsoft.EntityFrameworkCore;
using System;

namespace DockerNetOrchester.Data
{
    public class Model : DbContext
    {
        public static string CS = string.Empty;

        public DbSet<AgentJob> AgentJobs { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(string.IsNullOrEmpty(CS) ? "Server=.;Database=DockerNetOrchester;User Id=dnninstaller;Password=dnninstaller;" : CS);
    }
}
