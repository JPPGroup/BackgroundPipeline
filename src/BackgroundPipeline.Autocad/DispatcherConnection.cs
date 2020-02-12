using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace BackgroundPipeline.Autocad
{
    public class DispatcherConnection : MessageBroker
    {
        private EventingBasicConsumer _consumer;
        private Dictionary<Guid, TaskCompletionSource<RemoteTask>> _awaitedResponses;

        public DispatcherConnection(string hostname, string username, string password)
        {
            EstablishObjects(hostname, username, password);
            _awaitedResponses = new Dictionary<Guid, TaskCompletionSource<RemoteTask>>();

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                RemoteTask response = JsonConvert.DeserializeObject<RemoteTask>(Encoding.UTF8.GetString(ea.Body));
                
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

            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(remoteTask));
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
