using OrderProducer.Connections.Repository;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace OrderProcessor.Processing
{
    public interface IMessageBroker : IDisposable
    {
        public Task SendOrders(List<OrderOutbox> outboxOrders);
        public void Dispose();
        public void Connect();
    }

    public class Broker : IMessageBroker
    {
        private IConnection _connection;
        private IChannel _channel;
        private string _queueName;
        private string _connectionString;

        public Broker(string queueName, string connectionString)
        {
            this._queueName = queueName;
            this._connectionString = connectionString;
        }

        public static Broker BrokerFactory(string queueName, string connectionString)
        {
            Broker broker = new Broker(queueName, connectionString);
            broker.Connect();
            return broker;
        }

        public void Connect()
        {
            Task task = ConnectAsync();
            task.Wait();
        }

        async private Task ConnectAsync()
        {
            var connfactory = new ConnectionFactory
            {
                Uri = new Uri(this._connectionString)
            };
            _connection = await connfactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(queue: this._queueName, exclusive: false);
        }

        public void Dispose()
        {
            _connection.CloseAsync();
        }

        async public Task SendOrders(List<OrderOutbox> outboxOrders)
        {
            foreach (OrderOutbox order in outboxOrders)
            {
                await SendOrder(order);
            }
        }
        async private Task SendOrder(OrderOutbox order)
        {
            var orderJson = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(orderJson);
            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: this._queueName, body: body);
        }
    }
}
