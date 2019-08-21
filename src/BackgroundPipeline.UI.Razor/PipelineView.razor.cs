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

        public bool Expanded { get; set; }

        public int MaxHeight { get; set; }

        public string StatusColor { get; set; }

        protected override async Task OnInitAsync()
        {
            Pipeline = await _coordinator.GetPipelineAsync(PipelineID);
            Pipeline.PropertyChanged += Pipeline_PropertyChanged;
        }

        private void Pipeline_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(Pipeline.Status)
            {
                case Status.Completed:
                case Status.Running:
                    StatusColor = "#d9ff8c";
                    break;

                case Status.Stopped:
                    StatusColor = "#b52a2a";
                    break;
            }
        }

        protected void ToggleCollapse()
        {
            Expanded = !Expanded;
            MaxHeight = Expanded ? 300 : 0;
        }
    }
}
