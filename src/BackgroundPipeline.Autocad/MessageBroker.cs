using Jpp.Common;
using RabbitMQ.Client;

namespace BackgroundPipeline.Autocad
{
    public class MessageBroker : DisposableManagedObject
    {
        public const string EXCHANGE_NAME = "drafter_work";
        public const string RESPONSE_EXCHANGE_NAME = "drafter_response";
        public const string AUTOCAD_QUEUE = "drafter_work_autocad_v1";

        protected IConnection _connection;
        protected IModel _channel;
        protected IBasicProperties _properties;

        protected void EstablishObjects(string hostname, string username, string password)
        {
            var factory = new ConnectionFactory() {HostName = hostname, UserName = "jpp", Password = "jpp"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(EXCHANGE_NAME, "topic");
            _channel.QueueDeclare(AUTOCAD_QUEUE, true, false, false, null);
            _channel.QueueBind(AUTOCAD_QUEUE, EXCHANGE_NAME, "autocad.v1");

            _channel.ExchangeDeclare(RESPONSE_EXCHANGE_NAME, "fanout");
            _channel.QueueDeclare("drafter_response", true, false, false, null);
            _channel.QueueBind("drafter_response", RESPONSE_EXCHANGE_NAME, "#");

            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true;
            _properties.ContentType = "application/json";

            _channel.BasicQos(0, 1, false);
        }

        protected override void DisposeManagedResources()
        {
            _connection.Dispose();
            _channel.Dispose();
        }

        public uint AutocadWorkQueueLength()
        {
            return _channel.MessageCount(AUTOCAD_QUEUE);
        }
    }
}
