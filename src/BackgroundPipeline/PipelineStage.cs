using Jpp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public abstract class PipelineStage : BaseNotify
    {
        public Guid ID { get; set; }
        public string StageName { get; set; }
        public Status Status
        {
            get { return _status; }
            set { SetField(ref _status, value, nameof(Status)); }
        }

        private Status _status;

        public Guid PiplelineID {get; set; }
        public virtual Pipeline Pipeline { get; set; }

        public Guid NextStageID { get; set; }

        public DateTime LastRun { get; set; }

        public async Task Run()
        {
            Status = await RunPayload();
            LastRun = DateTime.Now;
        }

        protected abstract Task<Status> RunPayload();
    }
}
