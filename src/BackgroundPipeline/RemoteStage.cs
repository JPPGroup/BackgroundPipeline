using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public abstract class RemoteStage : PipelineStage
    {
        public IRemoteTask WorkRemoteTask { get; set; }

        private IRemoteConnection _connection;
        private bool _responseRecieved { get; set; } = false;

        public RemoteStage(IRemoteConnection connection, Pipeline parent, string name) : base(parent, name)
        {
            _connection = connection;
        }

        protected override async Task<Status> RunPayloadAsync()
        {
            if (!_responseRecieved)
            {
                if (WorkRemoteTask == null)
                    return Status.Error;

                Status status = await PrepareRemote();

                if (status != Status.Running)
                    return status;
                
                _connection.SendMessage(WorkRemoteTask);

                _ = Task.Run(async () =>
                  {
                      WorkRemoteTask = await _connection.GetResponseAsync(WorkRemoteTask.Id);
                      _responseRecieved = true;
                      this.Status = Status.Queued;
                  });

                return Status.RemoteRunning;
            }
            else
            {
                //Process task
                if (!WorkRemoteTask.ResponseStatus.HasValue)
                    return Status.Error;

                return await RecieveRemote();
            }
        }

        protected abstract Task<Status> PrepareRemote();

        protected abstract Task<Status> RecieveRemote();
    }
}
