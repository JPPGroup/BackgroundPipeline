using Jpp.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public class Pipeline : BaseNotify
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Status Status {
            get { return _status; }
            set { SetField(ref _status, value, nameof(Status)); }
        }

        private Status _status;

        public DateTime RequiredDate { get; set; }
        public DateTime QueuedDate { get; set; }
        public Status LastAdvanced { get; set; }

        public Priority Priority { get; set; }

        public Guid CurrentStageID { get; set; }
        public virtual PipelineStage CurrentStage { get; set; }

        public virtual ICollection<PipelineStage> Stages { get; set; }
    }

    public enum Status
    {
        Queued,
        Running,
        Paused,
        Stopped,
        Completed
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
        VeryHigh,
        Force
    }
}
