using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace DockerNetOrchester.Data
{
    public class AgentJob
    {
        public Guid Id { get; set; }
        public string AgentName { get; set; }
        public Guid JobId { get; set; }
    }
}
