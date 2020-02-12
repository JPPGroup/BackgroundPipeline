using Jpp.BackgroundPipeline.Stages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.UI.Razor.Stages
{
    public class AzureBlobStorageUploadStageViewBase : ComponentBase
    {
        [Parameter]
        public AzureBlobUploadStage Stage { get; set; }

        public string StatusColor { get; set; }

        public int MaxChunks { get; set; }
        public int CurrentChunk { get; set; }

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
            }

            MaxChunks = Stage.MaxChunks;
            CurrentChunk = Stage.CurrentChunk;
        }
    }
}
