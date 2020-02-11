using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace BackgroundPipeline.Autocad
{
    public class DispatcherConnection : MessageBroker
    {
        private EventingBasicConsumer _consumer;
        private Dictionary<Guid, TaskCompletionSource<RemoteTask>> _awaitedResponses;

        public DispatcherConnection(string hostname)
        {
            EstablishObjects(hostname);
            _awaitedResponses = new Dictionary<Guid, TaskCompletionSource<RemoteTask>>();

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                RemoteTask response = JsonSerializer.Deserialize<RemoteTask>(ea.Body);
                
                //do something
                if (_awaitedResponses.ContainsKey(response.Id))
                {
                    _awaitedResponses[response.Id].SetResult(response);
                }
                else
                {
                    throw new NotImplementedException("Unknown message - consider buffering");
                }
            };
        }

        public void SendMessage(RemoteTask remoteTask)
        {
            string routingKey;

            switch (remoteTask.WorkerRuntime)
            {
                case Runtime.Autocad:
                    routingKey = "autocad.v1";
                    break;
                
                default:
                    throw new NotImplementedException();
            }

            byte[] data = JsonSerializer.SerializeToUtf8Bytes(remoteTask);
            _properties.CorrelationId = remoteTask.Id.ToString();

            _channel.BasicPublish(EXCHANGE_NAME, routingKey, true, _properties, data);
        }

        public async Task<RemoteTask> GetResponseAsync(Guid responseId)
        {
            TaskCompletionSource<RemoteTask> taskCompletion = new TaskCompletionSource<RemoteTask>();
            _awaitedResponses.Add(responseId, taskCompletion);

            RemoteTask response = await taskCompletion.Task;
            return response;
        }
    }
}
