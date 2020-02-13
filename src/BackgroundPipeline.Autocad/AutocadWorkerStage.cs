using System.Collections.Generic;
using System.Threading.Tasks;
using Jpp.BackgroundPipeline;

namespace BackgroundPipeline.Autocad
{
    public class AutocadWorkerStage : RemoteStage
    {
      
        public AutocadWorkerStage(DispatcherConnection connection) : base(connection)
        { }

        protected async override Task<Status> PrepareRemote()
        {
            if (_inputs.ContainsKey("WorkingFiles"))
                WorkRemoteTask.WorkingDirectory = (List<File>)_inputs["WorkingFiles"];

            return Status.Running;
        }

        protected async override Task<Status> RecieveRemote()
        {
            switch (WorkRemoteTask.ResponseStatus.Value)
            {
                case ResponseStatus.OK:
                    Output["WorkingFiles"] = WorkRemoteTask.WorkingDirectory;
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
