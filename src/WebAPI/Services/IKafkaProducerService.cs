using Domain.Models;

namespace WebApi.Services
{
    public interface IKafkaProducerService
    {
        Task PublishMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken);
        Task PublishMessagePackMessageAsync(KafkaMessageRequest request, CancellationToken cancellationToken);
    }
}
