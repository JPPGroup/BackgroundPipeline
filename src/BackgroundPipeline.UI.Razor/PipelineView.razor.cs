using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineViewBase : ComponentBase
    {
        [Inject]
        protected PipelineCoordinator _coordinator { get; set; }

        [Parameter]
        public Guid PipelineID { get; set; }

        [Parameter]
        public Pipeline Pipeline { get; set; }

        public string PipelineName { get; set; } = "Test Name";

        protected override async Task OnInitAsync()
        {
            Pipeline = await _coordinator.GetPipelineAsync(PipelineID);
        }
    }
}
