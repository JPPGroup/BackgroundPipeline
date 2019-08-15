using Microsoft.EntityFrameworkCore;

namespace Jpp.BackgroundPipeline.Storage
{
    public interface IPipelineDBContext
    {
        /// <summary>
        /// All pipelines
        /// </summary>
        DbSet<Pipeline> Pipelines { get; set; }
        
        /// <summary>
        /// All pipeline stage
        /// </summary>
        DbSet<PipelineStage> PipelineStages { get; set; }
        
        /// <summary>
        /// Manually request save
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
