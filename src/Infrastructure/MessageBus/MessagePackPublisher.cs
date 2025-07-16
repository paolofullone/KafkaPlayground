using Confluent.Kafka;
using Infrastructure.MessageBus.Interfaces;
using System.Text;

namespace Infrastructure.MessageBus;
public class MessagePackPublisher(ProducerConfig producerConfig) : IMessagePackPublisher
{
    private readonly IProducer<Null, byte[]> _producer = new ProducerBuilder<Null, byte[]>(producerConfig).Build();
    private const string CreateDateHeader = "created-date-time";

    public async Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default)
    {
        // add try/catch, logs...
        var value = MessagePackFormat.Serialize<T>(message);

        await _producer.ProduceAsync(topic,
            new Message<Null, byte[]>
            {
                Value = value,
                Headers = new Headers
                {
                    new Header(CreateDateHeader, Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString("o")))
                }
            }, cancellationToken);

    }
}

