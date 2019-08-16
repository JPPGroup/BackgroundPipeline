using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            await _pipelines.SavePipeline(pipeline);
        }
    }
}
