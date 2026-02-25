using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;
using System.Net;
using System.Text;

// Stream systems are entry points for RabbitMQ stream client
var config = new StreamSystemConfig();
var streamSystem = await StreamSystem.Create(config);

// Create and configure a new stream
await streamSystem.CreateStream(new StreamSpec("hello-stream")
{
    MaxLengthBytes = 5_000_000_000
});

// Create a producer and bind it to a stream
var producer = await Producer.Create(new ProducerConfig(streamSystem, "hello-stream"));

await producer.Send(new Message(Encoding.UTF8.GetBytes($"Hello, World")));

Console.WriteLine(" [x] Send 'Hello, World!'");

Console.WriteLine(" [x] Press any key to exit");
Console.ReadKey();
await producer.Close();
await streamSystem.Close();