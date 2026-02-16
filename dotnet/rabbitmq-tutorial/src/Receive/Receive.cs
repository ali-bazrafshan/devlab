using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost",
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// Create a new exchange. This operation is idempotent
await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);

// Create a temporary queue with a random name
var queue = await channel.QueueDeclareAsync();

// Bind the exchange to the queue
await channel.QueueBindAsync(queue: queue.QueueName, exchange: "logs", routingKey: string.Empty);

// Register a consumer
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($" [x] Received {message}");

    int dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 1000);

    Console.WriteLine(" [x] Done");

    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(
    queue: queue.QueueName,
    autoAck: false,
    consumer: consumer
);

// Wait for messages
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();