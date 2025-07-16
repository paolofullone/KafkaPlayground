using Application.Services;
using Domain.Models;
using Infrastructure.MessageBus.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Worker
{
    class KafkaWorker(
        IMessageConsumer<SampleMessage> consumer,
        IKafkaPlaygroundConsumerService service) : BackgroundService
    {

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("starting KafkaWorker...");

            await consumer.StartConsumer(cancellationToken);

            await Task.FromResult(base.StartAsync(cancellationToken));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumer.ConsumeAsync(service.DoSomethingWithMessage, stoppingToken);

            Console.WriteLine("Stoping KafkaWorkers...");
        }

    }
}