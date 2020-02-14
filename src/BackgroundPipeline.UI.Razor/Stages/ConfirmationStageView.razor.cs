using Jpp.BackgroundPipeline.Stages;
using Microsoft.AspNetCore.Components;

namespace Jpp.BackgroundPipeline.UI.Razor.Stages
{
    public class ConfirmationStageViewBase : GenericStageViewBase
    {
        public void Confirm()
        {
            ((ConfirmationStage)Stage).Confirmed = true;
        }
    }
}
