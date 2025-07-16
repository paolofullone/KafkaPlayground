using Application.Services;
using Confluent.Kafka;
using Domain.Models;
using Infrastructure.MessageBus;
using Infrastructure.MessageBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.Extension;
public static class KafkaConsumerExtensions
{
    public static IServiceCollection AddKafkaConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        var consumerConfig = configuration.GetSection("KafkaSettings:ConsumerConfig").Get<ConsumerConfig>();
        var parallelConsumers = configuration.GetSection("KafkaSettings:ParallelConsumers").Get<int>();
        var topic = configuration.GetSection("KafkaSettings:Topics:KafkaPublisher:Name").Get<string>();
        var messagePackTopic = configuration.GetSection("KafkaSettings:Topics:KafkaMessagePackPublisher:Name").Get<string>();

        consumerConfig.GroupId = Guid.NewGuid().ToString();

        services.AddSingleton<IMessageConsumer<SampleMessage>>(sp =>
            new KafkaMessageConsumer<SampleMessage>(consumerConfig, topic, parallelConsumers));

        services.AddSingleton<IMessageConsumer<MessagePackSampleMessage>>(sp =>
            new KafkaMessagePackConsumer<MessagePackSampleMessage>(consumerConfig, messagePackTopic, parallelConsumers));

        services.AddSingleton<IKafkaPlaygroundConsumerService, KafkaPlaygroundConsumerService>();

        return services;
    }

    public static IServiceCollection AddKafkaWorkers(this IServiceCollection services)
    {
        services.AddHostedService<KafkaWorker>();
        services.AddHostedService<KafkaMessagePackWorker>();
        return services;
    }
}
