﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    /// <summary>
    /// Background service responsible for running and managing active pipelines
    /// </summary>
    public class PipelineRunner : BackgroundService
    {
        private readonly PipelineCoordinator _pipelineCoordinator;

        /// <summary>
        /// Create new instance of pipeline runner
        /// </summary>
        /// <param name="coord">Coordinator with active pipelines</param>
        public PipelineRunner(PipelineCoordinator coord)
        {
            _pipelineCoordinator = coord;
        }

        /// <summary>
        /// Execution loop
        /// </summary>
        /// <param name="stoppingToken">Token to request graceful cancellation</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Blocking request to retrieve next pipeline
                Pipeline pipe = await _pipelineCoordinator.GetNextQueuedAsync().ConfigureAwait(false);
                pipe.Status = Status.Running;           
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (pipe.Status == Status.Running)
                    {
                        await pipe.CurrentStage.RunAsync(pipe.OutputCache).ConfigureAwait(false);
                        
                        //Set overall pipeline status based on return
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

        /// <summary>
        /// Helper to move the pipeliane to the next stage in the queue
        /// </summary>
        /// <param name="pipe">Pipeline to act on</param>
        private static void AdvanceStage(Pipeline pipe)
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

        /// <summary>
        /// Helper for transferring output between stages
        /// </summary>
        /// <param name="pipe">Pipeline to act on</param>
        private static void PersistOutputs(Pipeline pipe)
        {
            foreach((string key, object value) in pipe.CurrentStage.Output)
            {
                if (pipe.OutputCache.ContainsKey(key))
                {
                    pipe.OutputCache[key] = value;
                }
                else
                {
                    pipe.OutputCache.Add(key, value);
                }
            }
        }
    }
}
