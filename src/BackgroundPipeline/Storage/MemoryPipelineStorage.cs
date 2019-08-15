using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.Storage
{
    public class MemoryPipelineStorage : IPipelineStorage
    {
        private Dictionary<Guid, Pipeline> _pipelines;

        public MemoryPipelineStorage()
        {
            _pipelines = new Dictionary<Guid, Pipeline>();
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelinesAsync()
        {            
            return _pipelines.Values;
        }

        public async Task<Pipeline> GetPipelineAsync(Guid id)
        {
            if (_pipelines.ContainsKey(id))
                return _pipelines[id];

            return null;
        }

        public async Task SavePipelineAsync(Pipeline pipeline)
        {
            _pipelines[pipeline.ID] = pipeline;
        }
    }
}
