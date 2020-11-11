using System;
using System.Collections.Generic;
using System.Text;

namespace DockerNetOrchester.Data
{
    public class Job
    {
        public Guid Id { get; set; }
        public bool Done { get; set; }
    }
}
