using Microsoft.Extensions.Configuration;
using OrderProcessor.Processing;
using OrderProcessor.Connections.Repository;
using Microsoft.EntityFrameworkCore;

var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables()
           .Build();

var messageBroker = Broker.BrokerFactory(
    configuration["queueName"] ?? throw new ArgumentNullException(),
    configuration.GetConnectionString("messagebroker") ?? throw new ArgumentNullException()
);

var outboxDbOptions = new DbContextOptionsBuilder<OrderContext>()
                .UseSqlServer(configuration.GetConnectionString("db") ?? throw new ArgumentNullException())
                .Options;
var outboxDb = new OrderContext(outboxDbOptions);

var producer = new Producer(outboxDb, messageBroker);


var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};
await producer.ForeverFetchAndProcessOrders(cts.Token);