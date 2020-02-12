using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineFactorySelectorBase : ComponentBase
    {
        [Inject]
        protected PipelineCoordinator _coordinator { get; set; }

        [Inject] 
        private NavigationManager _navigation { get; set; }

        protected async void Create(IPipelineFactory factory)
        {
            Pipeline pipeline = await factory.Create();

            if (pipeline != null)
            {
                await _coordinator.QueuePipelineAsync(pipeline);
                _navigation.NavigateTo("running");
            }
        }
    }
}
