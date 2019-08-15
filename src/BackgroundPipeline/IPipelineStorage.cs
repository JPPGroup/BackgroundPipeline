using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    interface IPipelineStorage
    {
        IEnumerable<Pipeline> GetAllPipelines();

        Pipeline GetPipeline(Guid id);

        void SavePipeline(Guid id);
    }
}
