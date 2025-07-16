using Domain.Models;
using Infrastructure.MessageBus.Interfaces;
using Microsoft.Extensions.Options;
using WebApi.Settings;

namespace WebApi.Services
{
    public class KafkaProducerService(IMessagePublisher publisher, IMessagePackPublisher messagePackPublisher, IOptions<TopicSettings> options) : IKafkaProducerService
    {
        public async Task PublishMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                for (var i = 0; i < request.MessageAmount; i++)
                {
                    var message = new SampleMessage
                    {
                        MessageId = Guid.NewGuid(),
                        MessageDate = DateTime.Now
                    };

                    await publisher.PublishAsync(message, options.Value.KafkaPublisher.Name, cancellationToken);
                }
            });
        }

        public async Task PublishMessagePackMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                for (var i = 0; i < request.MessageAmount; i++)
                {
                    var message = new MessagePackSampleMessage
                    {
                        Id = Guid.NewGuid(),
                        MessageDate = DateTime.UtcNow,
                        RandomPrice = new Random().Next(1, 10_000) / 3,
                        RandomQuantity = new Random().Next(1, 10_000),
                    };

                    await messagePackPublisher.PublishAsync(message, options.Value.KafkaMessagePackPublisher.Name, cancellationToken);
                }
            });
        }
    }
}
