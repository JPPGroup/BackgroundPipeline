using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.Storage
{
    public interface IPipelineStorage
    {
        Task<IEnumerable<Pipeline>> GetAllPipelinesAsync();

        Task<Pipeline> GetPipelineAsync(Guid id);

        Task SavePipelineAsync(Pipeline pipeline);
    }
}
