using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost",
};
// A connection represents a TCP connection between application and RabbitMQ broker
using var connection = await factory.CreateConnectionAsync();
// A channel represents a virtual connection. Each connection can create multiple channels
using var channel = await connection.CreateChannelAsync();

// Create a queue. Idempotent: declaring an already existing queue will do nothing
await channel.QueueDeclareAsync(
    queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

// Publish a message
await channel.BasicPublishAsync(
    exchange: string.Empty,
    routingKey: "hello",
    body: Encoding.UTF8.GetBytes("Hello World!")
);