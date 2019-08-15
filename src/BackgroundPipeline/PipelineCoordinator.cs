using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    class PipelineCoordinator
    {
        private IPipelineStorage _pipelines;

        public PipelineCoordinator(IPipelineStorage storage)
        {
            _pipelines = storage;
        }
    }
}
