using Jpp.BackgroundPipeline.Stages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.UI.Razor.Stages
{
    public class FileUploadStageViewBase : ConfirmationStageViewBase
    {
        public bool FileNotReady { get; set; } = true;


        public void UploadComplete(byte[] fileData)
        {
            FileNotReady = false;
            (Stage as FileUploadStage).Data = fileData;
            (Stage as FileUploadStage).Filename = Guid.NewGuid().ToString();
        }
    }
}
