﻿using Jpp.BackgroundPipeline.Stages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineCoordinatorViewBase : ComponentBase
    {
        [Inject]
        protected PipelineCoordinator _coordinator { get; set; }
                
        public IEnumerable<Pipeline> Pipelines { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Pipelines = await _coordinator.GetAllPipelinesAsync();
        }

        protected async void AddPipeline()
        {
            /*Pipeline p = new Pipeline()
            {
                ID = Guid.NewGuid(),
                Name = "New Pipeline",
                Status = Status.Queued
            };

            ConfirmationStage cs3 = new ConfirmationStage();
            cs3.PipelineID = p.ID;
            cs3.Pipeline = p;

            ConfirmationStage cs2 = new ConfirmationStage();
            cs2.PipelineID = p.ID;
            cs2.Pipeline = p;
            cs2.NextStageID = cs3.ID;

            FileUploadStage cs = new FileUploadStage();
            cs.PipelineID = p.ID;
            cs.Pipeline = p;
            cs.NextStageID = cs2.ID;
            
            p.Stages.Add(cs);
            p.Stages.Add(cs2);
            p.Stages.Add(cs3);

            p.CurrentStageID = cs.ID;     

            await _coordinator.QueuePipelineAsync(p);            */
        }
    }
}
