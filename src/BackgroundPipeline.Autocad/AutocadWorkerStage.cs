using System.Collections.Generic;
using System.Threading.Tasks;
using Jpp.BackgroundPipeline;

namespace BackgroundPipeline.Autocad
{
    public class AutocadWorkerStage : RemoteStage
    {
      
        public AutocadWorkerStage(DispatcherConnection connection, Pipeline parent, string name) : base(connection, parent, name)
        { }

        protected async override Task<Status> PrepareRemote()
        {
            WorkRemoteTask.WorkingDirectory = Pipeline.Artifacts.GetFiles();

            return Status.Running;
        }

        protected async override Task<Status> RecieveRemote()
        {
            switch (WorkRemoteTask.ResponseStatus.Value)
            {
                case ResponseStatus.OK:
                    Pipeline.Artifacts.SetFiles(WorkRemoteTask.WorkingDirectory);
                    return Status.Completed;

                case ResponseStatus.UnknownTask:
                    return Status.Error;

                case ResponseStatus.UnkownFailure:
                    return Status.Error;

                default:
                    return Status.Error;
            }
        }
    }
}
