using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public class Pipeline
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Queued,
        Running,
        Paused,
        Stopped,
        Completed
    }
}
