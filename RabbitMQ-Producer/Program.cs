using RabbitMQ.Client;
using System.Text;

var QUEUE_NAME = "myFirstQueue";
var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: QUEUE_NAME,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);

    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;

    channel.BasicPublish(exchange: "",
                         routingKey: QUEUE_NAME,
                         basicProperties: properties,
                         body: body);

    Console.WriteLine(" [x] Sent {0}", message);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadKey();


static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Default message!");
}