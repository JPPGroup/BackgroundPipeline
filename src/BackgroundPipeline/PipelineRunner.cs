using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    class PipelineRunner : BackgroundService
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
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (pipe.Status == Status.Running)
                    {
                        pipe.CurrentStage.Run();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
