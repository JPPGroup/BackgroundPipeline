using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public class MemoryPipelineStorage : IPipelineStorage
    {
        private Dictionary<Guid, Pipeline> _pipelines;

        public MemoryPipelineStorage()
        {
            _pipelines = new Dictionary<Guid, Pipeline>();
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelines()
        {            
            return _pipelines.Values;
        }

        public async Task<Pipeline> GetPipeline(Guid id)
        {
            if (_pipelines.ContainsKey(id))
                return _pipelines[id];

            return null;
        }

        public async Task SavePipeline(Pipeline pipeline)
        {
            _pipelines[pipeline.ID] = pipeline;
        }
    }
}
