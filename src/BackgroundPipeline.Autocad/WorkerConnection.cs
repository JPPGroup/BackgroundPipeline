using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace BackgroundPipeline.Autocad
{
    public class WorkerConnection : MessageBroker
    {
        private EventingBasicConsumer _consumer;

        private TaskCompletionSource<RemoteTask> _nextMessage;

        public WorkerConnection(string hostname, string username, string password)
        {
            EstablishObjects(hostname, username, password);
            
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                // TODO: Consider buffering
                if(_nextMessage.Task.IsCompleted)
                    throw new ArgumentOutOfRangeException();

                var body = ea.Body;
                RemoteTask response = JsonConvert.DeserializeObject<RemoteTask>(Encoding.UTF8.GetString(ea.Body));
                _nextMessage.SetResult(response);
            };
        }

        public void SendResponse(RemoteTask remoteTask)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(remoteTask));
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
