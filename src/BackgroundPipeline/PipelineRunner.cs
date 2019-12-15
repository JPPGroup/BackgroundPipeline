using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public class PipelineRunner : BackgroundService
    {
        PipelineCoordinator _pipelineCoordinator;

        public PipelineRunner(PipelineCoordinator coord)
        {
            _pipelineCoordinator = coord;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Pipeline pipe = await _pipelineCoordinator.GetNextQueued();
                pipe.Status = Status.Running;                
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (pipe.Status == Status.Running)
                    {
                        await pipe.CurrentStage.Run(pipe.OutputCache);
                        
                        switch (pipe.CurrentStage.Status)
                        {
                            case Status.Completed:
                                AdvanceStage(pipe);
                                break;

                            case Status.Error:
                                pipe.Status = Status.Error;
                                break;

                            case Status.InputRequired:
                                pipe.Status = Status.InputRequired;
                                break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void AdvanceStage(Pipeline pipe)
        {
            PersistOutputs(pipe);

            if (pipe.CurrentStage.NextStageID != Guid.Empty)
            {
                pipe.CurrentStageID = pipe.CurrentStage.NextStageID;

            } else
            {
                pipe.Status = Status.Completed;
            }

            pipe.LastAdvanced = DateTime.Now;            
        }

        private void PersistOutputs(Pipeline pipe)
        {
            foreach(KeyValuePair<string, object> pair in pipe.CurrentStage.Output)
            {
                if (pipe.OutputCache.ContainsKey(pair.Key))
                {
                    pipe.OutputCache[pair.Key] = pair.Value;
                }
                else
                {
                    pipe.OutputCache.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
