using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost",
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);

// Publish a message
var message = GetMessage(args);

if (message != string.Empty)
{
    await channel.BasicPublishAsync(
        exchange: "logs",
        routingKey: string.Empty,
        body: Encoding.UTF8.GetBytes(message)
    );
    Console.WriteLine($" [x] Sent {message}");
}
else
{
    Console.WriteLine("Invalid input.");
}

static string GetMessage(string[] args)
{
    return string.Join(' ', args);
}