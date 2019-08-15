using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.Stages
{
    public class ConfirmationStage : PipelineStage
    {
        public bool Confirmed
        {
            get
            {
                return _confirmed;
            }
            set
            {
                if(value)
                {
                    Status = Status.Queued;
                    Pipeline.Status = Status.Queued;
                }
                _confirmed = value;
            }
        }
        private bool _firstRun = true;
        private bool _confirmed = false;

        protected async override Task<Status> RunPayloadAsync()
        {
            if (_firstRun)
            {
                Status = Status.InputRequired;                
                _firstRun = false;
            }

            if (Confirmed)
            {
                return Status.Completed;
            } else
            {
                return Status.InputRequired;
            }
        }
    }
}
