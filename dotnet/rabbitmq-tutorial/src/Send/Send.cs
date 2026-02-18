using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost",
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "topic_logs", type: ExchangeType.Topic);

// Publish a message
var result = GetMessage(args);

if (result.HasValue)
{
    await channel.BasicPublishAsync(
        exchange: "topic_logs",
        routingKey: result.Value.topic,
        body: Encoding.UTF8.GetBytes(result.Value.message)
    );
    Console.WriteLine($" [x] Sent {result.Value.message}");
}
else
{
    Console.WriteLine("Invalid input.");
}

static (string message, string topic)? GetMessage(string[] args)
{
    if (args.Length < 2) return null;

    var topic = args[0];
    var message = string.Join(' ', args[1..]);
    return (message, topic);
}