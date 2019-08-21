using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public abstract class PipelineStage
    {
        public Guid ID { get; set; }
        public string StageName { get; set; }
        public Status Status { get; set; }

        public Guid PiplelineID {get; set; }
        public virtual Pipeline Pipeline { get; set; }

        public DateTime LastRun { get; set; }

        public void Run()
        {
            Status = RunPayload();
            LastRun = DateTime.Now;
        }

        protected abstract Status RunPayload();
    }
}
