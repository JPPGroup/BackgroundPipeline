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

        public string StatusColor { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Pipeline = await _coordinator.GetPipelineAsync(PipelineID);
            Pipeline.PropertyChanged += Pipeline_PropertyChanged;

            UpdateDisplay();
        }

        private void Pipeline_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateDisplay();

            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private void UpdateDisplay()
        {
            switch (Pipeline.Status)
            {
                case Status.Completed:
                case Status.Running:
                    StatusColor = "#d9ff8c";
                    break;

                case Status.Stopped:
                    StatusColor = "#b52a2a";
                    break;

                case Status.InputRequired:
                    StatusColor = "#efa649";
                    break;
            }
        }
    }
}
