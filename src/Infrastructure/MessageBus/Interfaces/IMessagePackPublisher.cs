namespace Infrastructure.MessageBus.Interfaces
{
    public interface IMessagePackPublisher
    {
        Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default);
    }
}
