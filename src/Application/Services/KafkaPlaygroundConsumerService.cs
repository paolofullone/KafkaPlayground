using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class KafkaPlaygroundConsumerService(IDbSQLRepository sqlRepository) : IKafkaPlaygroundConsumerService
    {
        public async Task<bool> DoSomethingWithMessage(SampleMessage message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Json Message - {message.MessageId.ToString()}, {message.MessageDate.ToString()}");

            var checkInsertion = await sqlRepository.AddAsync(message, cancellationToken);

            return checkInsertion > 0;
        }

        public async Task DoSomethingWithMessagePackMessage(MessagePackSampleMessage message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MessagePack Message Date {message.MessageDate}");
            Console.WriteLine($"MessagePack Message Random Price {message.RandomPrice}");
            Console.WriteLine($"MessagePack Message Random Qty {message.RandomQuantity}");

            return; // just demonstration purposes
        }
    }
}
