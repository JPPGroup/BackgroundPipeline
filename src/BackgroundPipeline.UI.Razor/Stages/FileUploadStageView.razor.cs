using Jpp.BackgroundPipeline.Stages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Razor;

namespace Jpp.BackgroundPipeline.UI.Razor.Stages
{
    public class FileUploadStageViewBase : ConfirmationStageViewBase
    {
        public bool FileNotReady { get; set; } = true;


        public void UploadComplete(FileUploadEventArgs args)
        {
            FileNotReady = false;
            (Stage as FileUploadStage).AddFile(args.Filename, args.Data);
        }
    }
}
