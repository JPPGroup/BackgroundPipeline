using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public interface IPipelineStorage
    {
        Task<IEnumerable<Pipeline>> GetAllPipelines();

        Task<Pipeline> GetPipeline(Guid id);

        Task SavePipeline(Pipeline pipeline);
    }
}
