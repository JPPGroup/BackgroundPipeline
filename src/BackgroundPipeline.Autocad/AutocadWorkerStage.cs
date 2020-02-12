using System.Collections.Generic;
using System.Threading.Tasks;
using Jpp.BackgroundPipeline;
using Jpp.Ironstone.Draughter.TaskPayloads;

namespace BackgroundPipeline.Autocad
{
    public class AutocadWorkerStage : PipelineStage
    {
        public const uint QUEUE_CUTOFF = 5;

        private DispatcherConnection _connection;

        public RemoteTask WorkRemoteTask { get; set; }

        public AutocadWorkerStage(DispatcherConnection connection)
        {
            _connection = connection;
        }

        protected override async Task<Status> RunPayloadAsync()
        {
            if (WorkRemoteTask == null)
                return Status.InputRequired;

            WorkRemoteTask.WorkingDirectory = (List<File>) _inputs["WorkingFiles"];

            if (_connection.AutocadWorkQueueLength() > QUEUE_CUTOFF)
                return Status.Queued; // TODO: Verify this doesnt break anything

            _connection.SendMessage(WorkRemoteTask);

            RemoteTask response = await _connection.GetResponseAsync(WorkRemoteTask.Id);

            //Process task
            if (!response.ResponseStatus.HasValue)
                return Status.Error;

            switch (response.ResponseStatus.Value)
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
