using Jpp.Common.Razor.Services;
using Microsoft.AspNetCore.Components;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineFactorySelectorBase : ComponentBase
    {
        [Inject]
        protected PipelineCoordinator _coordinator { get; set; }

        [Inject] 
        private NavigationManager _navigation { get; set; }

        [Inject]
        private ModalService _modal { get; set; }

        [Inject]
        private IArtifactPersistence _artifactPersistence { get; set; }

        protected async void Create(IPipelineFactory factory)
        {
            Pipeline pipeline = await factory.Create(_modal as ModalWrapper, _artifactPersistence);

            if (pipeline != null)
            {
                await _coordinator.QueuePipelineAsync(pipeline);
                _navigation.NavigateTo("running");
            }
        }
    }
}
