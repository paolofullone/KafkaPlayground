using Application.Services;
using Domain.Models;
using Infrastructure.MessageBus.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Worker
{
    class KafkaMessagePackWorker(
        IMessageConsumer<MessagePackSampleMessage> consumer,
        IKafkaPlaygroundConsumerService service) : BackgroundService
    {

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("starting KafkaMessagePackWorker...");

            await consumer.StartConsumer(cancellationToken);

            await Task.FromResult(base.StartAsync(cancellationToken));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumer.ConsumeAsync(service.DoSomethingWithMessagePackMessage, stoppingToken);

            Console.WriteLine("Stoping KafkaMessagePackWorkers...");
        }

    }
}