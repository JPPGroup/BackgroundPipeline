using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public interface IPipelineFactory
    {
        Task<Pipeline> Create();

        string Name { get; }

        string Description { get; }
    }
}
