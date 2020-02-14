using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public interface IPipelineFactory
    {
        Task<Pipeline> Create(IModal modal, IArtifactPersistence persistence);

        string Name { get; }

        string Description { get; }
    }
}
