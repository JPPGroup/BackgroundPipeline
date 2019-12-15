using Jpp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public class Pipeline : BaseNotify
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Status Status {
            get { return _status; }
            set { SetField(ref _status, value, nameof(Status)); }
        }

        private Status _status;

        public DateTime RequiredDate { get; set; }
        public DateTime QueuedDate { get; set; }
        public DateTime LastAdvanced { get; set; }

        public Priority Priority { get; set; }

        public Guid CurrentStageID 
        {
            get
            {
                return _currentStageID;
            }
            set
            {
                if(Stages.Where(ps => ps.ID == value).Count() > 0)
                {
                    _currentStageID = value;
                } else
                {
                    throw new ArgumentOutOfRangeException("Stage ID is not recognised");
                }
            }
        }

        private Guid _currentStageID;
        [NotMapped]
        public virtual PipelineStage CurrentStage 
        { 
            get
            {
                return Stages.Where(ps => ps.ID == CurrentStageID).First();
            }
            set
            {
                CurrentStageID = value.ID;
            }
        }               

        public virtual ICollection<PipelineStage> Stages { get; set; }    
        
        public Pipeline()
        {
            ID = Guid.NewGuid();
            Stages = new List<PipelineStage>();
        }

        [NotMapped]
        public Dictionary<string, object> OutputCache { get; set; }
    }

    public enum Status
    {
        Queued,
        Running,
        Paused,
        Stopped,
        Completed,
        InputRequired,
        Error
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
