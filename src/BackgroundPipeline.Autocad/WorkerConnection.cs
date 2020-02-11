using System;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace BackgroundPipeline.Autocad
{
    public class WorkerConnection : MessageBroker
    {
        private EventingBasicConsumer _consumer;

        private TaskCompletionSource<RemoteTask> _nextMessage;

        public WorkerConnection(string hostname)
        {
            EstablishObjects(hostname);
            
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                // TODO: Consider buffering
                if(_nextMessage.Task.IsCompleted)
                    throw new ArgumentOutOfRangeException();

                var body = ea.Body;
                RemoteTask response = JsonSerializer.Deserialize<RemoteTask>(ea.Body);
                _nextMessage.SetResult(response);
            };
        }

        public void SendResponse(RemoteTask remoteTask)
        {
            byte[] data = JsonSerializer.SerializeToUtf8Bytes(remoteTask);
            _properties.CorrelationId = remoteTask.Id.ToString();

            _channel.BasicPublish(RESPONSE_EXCHANGE_NAME, "", false, _properties, data);
        }
        
        public async Task<RemoteTask> GetRemoteTask()
        {
            _nextMessage = new TaskCompletionSource<RemoteTask>();
            RemoteTask response = await _nextMessage.Task;
            return response;
        }
    }
}
