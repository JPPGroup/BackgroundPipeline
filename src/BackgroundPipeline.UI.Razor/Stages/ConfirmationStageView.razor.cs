using Jpp.BackgroundPipeline.Stages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.UI.Razor.Stages
{
    public class ConfirmationStageViewBase : ComponentBase
    {
        [Parameter]
        public ConfirmationStage Stage { get; set; }

        public bool Expanded { get; set; }

        public int MaxHeight { get; set; }

        public string StatusColor { get; set; }

        public void Confirm()
        {
            Stage.Confirmed = true;
        }

        protected override async Task OnInitializedAsync()
        {            
            Stage.PropertyChanged += Pipeline_PropertyChanged;
        }

        private void Pipeline_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        protected void ToggleCollapse()
        {
            Expanded = !Expanded;
            MaxHeight = Expanded ? 100 : 0;
        }
    }
}
