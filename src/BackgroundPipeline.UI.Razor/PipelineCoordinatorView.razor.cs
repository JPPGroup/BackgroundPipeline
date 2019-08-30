using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineCoordinatorViewBase : ComponentBase
    {
        [Inject]
        protected PipelineCoordinator _coordinator { get; set; }
                
        public IEnumerable<Pipeline> Pipelines { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Pipelines = await _coordinator.GetAllPipelinesAsync();
        }

        protected async void AddPipeline()
        {
            Pipeline p = new Pipeline()
            {
                ID = Guid.NewGuid(),
                Name = "New Pipeline",
                Status = Status.Queued
            };

            await _coordinator.QueuePipeline(p);            
        }
    }
}
