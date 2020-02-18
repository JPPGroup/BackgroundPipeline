using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jpp.Common.Razor.Services;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineViewBase : ComponentBase
    {
        [Inject]
        protected PipelineCoordinator _coordinator { get; set; }

        [Inject] 
        protected ModalService _modalService { get; set; }

        [Parameter]
        public Guid PipelineID { get; set; }

        [Parameter]
        public Pipeline Pipeline { get; set; }

        [Parameter]
        public bool Expanded { get; set; }

        public string PipelineName { get; set; } = "Test Name";

        public string StatusColor { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Pipeline = await _coordinator.GetPipelineAsync(PipelineID);
            Pipeline.PropertyChanged += Pipeline_PropertyChanged;

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

            _headerStyle = $"background-color: {StatusColor};";
        }

        protected async void ArtifactModal()
        {
            await _modalService.ShowAsync("Artifacts", typeof(ArtifactView),
                new KeyValuePair<string, object>("Pipeline", Pipeline));
        }
    }
}
