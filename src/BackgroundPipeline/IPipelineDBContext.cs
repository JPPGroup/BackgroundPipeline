using Jpp.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public interface IPipelineDBContext
    {
        DbSet<Pipeline> Pipelines { get; set; }
        DbSet<PipelineStage> PipelineStages { get; set; }
        int SaveChanges();
    }
}
