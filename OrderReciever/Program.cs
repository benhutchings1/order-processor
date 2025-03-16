using RabbitMQ.Client.Events;   
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .AddEnvironmentVariables()
           .Build();

string connectionString = configuration.GetConnectionString("messagebroker") ?? throw new ArgumentNullException();

var factory = new ConnectionFactory {
    Uri = new Uri (connectionString)
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: configuration["queueName"] ?? throw new ArgumentNullException(),
    exclusive: false
);
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    return Task.CompletedTask;
};


Console.WriteLine(" [*] Waiting for messages.");

await channel.BasicConsumeAsync(configuration["queueName"], autoAck: true, consumer: consumer);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();