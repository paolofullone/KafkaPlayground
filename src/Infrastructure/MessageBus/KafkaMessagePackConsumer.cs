
using Confluent.Kafka;
using Infrastructure.MessageBus.Interfaces;
using System.Text.Json;

namespace Infrastructure.MessageBus
{
    public class KafkaMessagePackConsumer<T> : IMessageConsumer<T>
    {
        private readonly List<(IConsumer<Null, byte[]> Consumer, int Id)> _consumers;
        private readonly string _topic;
        private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

        public KafkaMessagePackConsumer(ConsumerConfig consumerConfig, string topic, int parallelConsumers)
        {
            _consumers = new List<(IConsumer<Null, byte[]> Consumer, int Id)>();
            for (int i = 1; i <= parallelConsumers; i++)
            {
                _consumers.Add((new ConsumerBuilder<Null, byte[]>(consumerConfig).Build(), i));
                Console.WriteLine($"Creating KafkaMessagePackConsumer {i}");
            }
            _topic = topic;
        }

        public string Topic => _topic;

        public Task StartConsumer(CancellationToken cancellationToken)
        {
            foreach (var (consumer, _) in _consumers)
            {
                consumer.Subscribe(_topic);
            }
            return Task.CompletedTask;
        }

        public async Task ConsumeAsync(Func<T, CancellationToken, Task> handle, CancellationToken cancellationToken)
        {
            var tasks = _consumers.Select(consumerTuple => Task.Run(async () =>
            {
                var (consumer, id) = consumerTuple;
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var message = consumer.Consume(cancellationToken);

                        var consumeResult = MessagePackFormat.Deserialize<T>(message.Message.Value);

                        if (cancellationToken.IsCancellationRequested)
                            break;

                        Console.WriteLine($"KafkaMessagePackConsumer {id} consumed a message");
                        _ = Task.Run(() => handle(consumeResult!, cancellationToken));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"KafkaMessagePackConsumer {id} encountered an error: {ex}");
                    }
                }
            }));
            await Task.WhenAll(tasks.ToArray());
        }
    }
}
