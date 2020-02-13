using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jpp.BackgroundPipeline;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace BackgroundPipeline.Autocad
{
    public class DispatcherConnection : MessageBroker, IRemoteConnection
    {
        private EventingBasicConsumer _consumer;
        private Dictionary<Guid, TaskCompletionSource<IRemoteTask>> _awaitedResponses;

        public DispatcherConnection(string hostname, string username, string password)
        {
            EstablishObjects(hostname, username, password);
            _awaitedResponses = new Dictionary<Guid, TaskCompletionSource<IRemoteTask>>();

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                IRemoteTask response = JsonConvert.DeserializeObject<IRemoteTask>(Encoding.UTF8.GetString(ea.Body));
                
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

        public void SendMessage(IRemoteTask remoteTask)
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

        public async Task<IRemoteTask> GetResponseAsync(Guid responseId)
        {
            TaskCompletionSource<IRemoteTask> taskCompletion = new TaskCompletionSource<IRemoteTask>();
            _awaitedResponses.Add(responseId, taskCompletion);

            IRemoteTask response = await taskCompletion.Task;
            return response;
        }
    }
}
