using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

    //don't dispatch a new message to a worker until it has processed and acknowledged the previous one
    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body) ?? "";
        Console.WriteLine(" [x] Received {0}", message);

        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 1000);

        Console.WriteLine(" [x] Done");

        // manual Ack (autoAck = false)
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };

    channel.BasicConsume(queue: QUEUE_NAME,
                            //autoAck: true,
                            autoAck: false,
                            consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();

}