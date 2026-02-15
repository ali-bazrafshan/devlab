using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost",
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// Consumer also declares the queue, because it might not be created by a publisher yet
await channel.QueueDeclareAsync(
    queue: "test-queue",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

// Configure quality of service parameters: how many messages consumer can prefetch before acknowledging previous ones
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

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
    queue: "test-queue",
    autoAck: false,
    consumer: consumer
);

// Wait for messages
Console.ReadLine();