using Confluent.Kafka;
using Infrastructure.MessageBus.Interfaces;
using System.Text.Json;

namespace Infrastructure.MessageBus
{
    public class KafkaPublisher(ProducerConfig producerConfig) : IMessagePublisher
    {
        private readonly IProducer<Null, string> _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public async Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize(message, _options) }, cancellationToken);
        }
    }
}
