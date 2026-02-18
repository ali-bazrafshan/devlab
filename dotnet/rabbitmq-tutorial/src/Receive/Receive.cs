using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: {0} [topic ..]", Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

var factory = new ConnectionFactory
{
    HostName = "localhost",
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// Create a new exchange. This operation is idempotent
await channel.ExchangeDeclareAsync(exchange: "topic_logs", type: ExchangeType.Topic);

// Create a temporary queue with a random name
var queue = await channel.QueueDeclareAsync();

foreach (var arg in args)
{
    await channel.QueueBindAsync(queue: queue.QueueName, exchange: "topic_logs", routingKey: arg);
}

// Register a consumer
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($" [x] Received '{ea.RoutingKey}' : '{message}'");

    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: queue.QueueName, autoAck: true, consumer: consumer);

// Wait for messages
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();