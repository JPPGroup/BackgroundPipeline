using Jpp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Jpp.BackgroundPipeline.Properties;

namespace Jpp.BackgroundPipeline
{
    /// <summary>
    /// Structure representing a pipeline
    /// </summary>
    public class Pipeline : BaseNotify
    {
        /// <summary>
        /// Pipeline ID
        /// </summary>
        // TODO: Can this be readonly??
        [Key] public Guid ID { get; set; }

        /// <summary>
        /// Pipeline Name
        /// </summary>
        public string Name
        {
            get { return _name;}
            set { SetField(ref _name, value, nameof(Name)); }
        }

        private string _name;

        /// <summary>
        /// Pipeline status
        /// </summary>
        public Status Status {
            get { return _status; }
            set { SetField(ref _status, value, nameof(Status)); }
        }
        private Status _status;

        /// <summary>
        /// Date the pipeline is required to complete by
        /// </summary>
        public DateTime RequiredDate
        {
            get { return _requiredDate; }
            set { SetField(ref _requiredDate, value, nameof(RequiredDate)); }
        }
        /// <summary>
        /// Date the pipeline was originally queued
        /// </summary>
        public DateTime QueuedDate
        {
            get { return _queuedDate; }
            set { SetField(ref _queuedDate, value, nameof(QueuedDate)); }
        }
        /// <summary>
        /// Last time the pipeline successfully completed a stage
        /// </summary>
        public DateTime LastAdvanced
        {
            get { return _lastAdvanced; }
            set { SetField(ref _lastAdvanced, value, nameof(LastAdvanced)); }
        }
        private DateTime _requiredDate, _queuedDate, _lastAdvanced;

        /// <summary>
        /// General pipeline priority
        /// </summary>
        public Priority Priority
        {
            get { return _priority; }
            set { SetField(ref _priority, value, nameof(Priority)); }
        }
        private Priority _priority;


        /// <summary>
        /// Id of the current active stage
        /// </summary>
        public Guid CurrentStageID 
        {
            get { return _currentStageID; }
            set
            {
                if(Stages.Any(ps => ps.ID == value))
                {
                    _currentStageID = value;
                } else
                {
                    throw new ArgumentOutOfRangeException(nameof(CurrentStageID), Resources.Pipeline_Error_UnknownStage);
                }
            }
        }
        private Guid _currentStageID;

        [NotMapped]
        public virtual PipelineStage CurrentStage 
        { 
            get
            {
                return Stages.First(ps => ps.ID == CurrentStageID);
            }
            set
            {
                CurrentStageID = value.ID;
            }
        }               

        public virtual ICollection<PipelineStage> Stages { get; private set; }    
        
        public Pipeline()
        {
            ID = Guid.NewGuid();
            Stages = new List<PipelineStage>();
            Name = "Default Name";
            OutputCache = new Dictionary<string, object>();
        }

        public PipelineStage GetQueuedStage()
        {
            var queuedStages = Stages.Where(s => s.Status == Status.Queued);
            PipelineStage selectedStage = null;

            foreach (PipelineStage queuedStage in queuedStages)
            {
                if (!queuedStages.Any(s => s.NextStageID == queuedStage.ID))
                {
                    selectedStage = queuedStage;
                    break;
                }
            }

            return selectedStage;
        }

        public bool StagesComplete()
        {
            return Stages.All(s => s.Status != Status.Queued && s.Status != Status.RemoteRunning);
        }

        [NotMapped]
        public Dictionary<string, object> OutputCache { get; set; }
    }

    /// <summary>
    /// Pipeline Statuses
    /// </summary>
    public enum Status
    {
        Queued,
        Running,
        Paused,
        Stopped,
        Completed,
        InputRequired,
        RemoteRunning,
        Error
    }

    /// <summary>
    /// Pipeline Priorities
    /// </summary>
    public enum Priority
    {
        Low,
        Medium,
        High,
        VeryHigh,
        Force
    }
}
