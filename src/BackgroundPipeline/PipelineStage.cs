using Jpp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    /// <summary>
    /// Base implementation representing a pipeline stage
    /// </summary>
    public abstract class PipelineStage : BaseNotify
    {
        /// <summary>
        /// ID of stage
        /// </summary>
        [Key] public Guid ID { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Name of stage
        /// </summary>
        public string StageName { get; private set; }

        /// <summary>
        /// Stage status
        /// </summary>
        public Status Status
        {
            get { return _status; }
            set { SetField(ref _status, value, nameof(Status)); }
        }
        private Status _status;

        [NotMapped]
        public Dictionary<string, object> Output { get; set; } = new Dictionary<string, object>();
        
        public Guid PipelineID { get; private set; }

        public virtual Pipeline Pipeline { get; private set; }

        public Guid NextStageID { get; set; }        

        public DateTime LastRun { get; set; }

        protected Dictionary<string, object> _inputs;

        internal async Task RunAsync(Dictionary<string, object> inputs)
        {
            _inputs = inputs;
            Status = await RunPayloadAsync();
            LastRun = DateTime.Now;            
        }

        protected abstract Task<Status> RunPayloadAsync();

        public PipelineStage(Pipeline parent, string stageName)
        {
            Pipeline = parent;
            StageName = stageName;
        }
    }
}
