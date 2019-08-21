using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Jpp.BackgroundPipeline
{
    public class PipelineCoordinator
    {
        private IPipelineStorage _pipelines;

        public PipelineCoordinator(IPipelineStorage storage)
        {
            _pipelines = storage;
        }

        public async Task<Pipeline> GetPipelineAsync(Guid id)
        {
            return await _pipelines.GetPipeline(id);
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelinesAsync()
        {
            await Task.Delay(3000);
            return await _pipelines.GetAllPipelines();
        }

        public async Task QueuePipeline(Pipeline pipeline)
        {
            pipeline.QueuedDate = DateTime.Now;
            pipeline.Status = Status.Queued;
            await _pipelines.SavePipeline(pipeline);
        }

        public async Task<Pipeline> GetNextQueued()
        {
            IEnumerable<Pipeline> pipelines = await _pipelines.GetAllPipelines();
            Pipeline nextPipeline = pipelines.Where(p => p.Status == Status.Queued).OrderByDescending(p => p.Priority).ThenByDescending(p => p.RequiredDate).ThenByDescending(p => p.QueuedDate).First();
            return nextPipeline;
        }
    }
}
