using Jpp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public abstract class PipelineStage : BaseNotify
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string StageName { get; set; }
        public Status Status
        {
            get { return _status; }
            set { SetField(ref _status, value, nameof(Status)); }
        }

        [NotMapped]
        public Dictionary<string, object> Output { get; set; } = new Dictionary<string, object>();

        private Status _status;

        public Guid PipelineID { get; set; }
        public virtual Pipeline Pipeline { get; set; }

        public Guid NextStageID { get; set; }        

        public DateTime LastRun { get; set; }

        protected Dictionary<string, object> _inputs;

        public async Task Run(Dictionary<string, object> inputs)
        {
            _inputs = inputs;
            Status = await RunPayload();
            LastRun = DateTime.Now;            
        }

        protected abstract Task<Status> RunPayload();
    }
}
