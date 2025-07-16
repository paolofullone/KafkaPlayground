using Confluent.Kafka;
using Infrastructure.MessageBus;
using Infrastructure.MessageBus.Interfaces;
using WebApi.Services;
using WebApi.Settings;

namespace WebApi.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure TopicSettings
        var topicSettings = configuration.GetSection("KafkaSettings:Topics");
        services.Configure<TopicSettings>(topicSettings);

        // Configure Producer
        var producerSettings = configuration.GetSection("KafkaSettings:ProducerConfig");
        var producerConfig = producerSettings.Get<ProducerConfig>()!;

        // Register Kafka services
        services.AddSingleton<IMessagePublisher>(_ => new KafkaPublisher(producerConfig));
        services.AddSingleton<IMessagePackPublisher>(_ => new MessagePackPublisher(producerConfig));
        services.AddScoped<IKafkaProducerService, KafkaProducerService>();

        return services;
    }
}
