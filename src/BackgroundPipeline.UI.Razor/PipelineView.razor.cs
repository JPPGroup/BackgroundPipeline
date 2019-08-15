using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineViewBase : ComponentBase
    {
        public string PipelineName { get; set; } = "Test Name";
    }
}
