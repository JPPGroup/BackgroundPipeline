using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Jpp.BackgroundPipeline.UI.Razor.Stages
{
    public class GenericStageViewBase : ComponentBase
    {
        [Parameter]
        public PipelineStage Stage { get; set; }

        public string StatusColor { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Stage.PropertyChanged += Pipeline_PropertyChanged;
            UpdateDisplay();
        }

        protected string _headerStyle;

        protected override async Task OnParametersSetAsync()
        {
            _headerStyle = $"background-color: {StatusColor};";
        }

        private void Pipeline_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateDisplay();

            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        protected void UpdateDisplay()
        {
            switch (Stage.Status)
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

                case Status.RemoteRunning:
                    StatusColor = "#cde3a8";
                    break;
            }
        }
    }
}
