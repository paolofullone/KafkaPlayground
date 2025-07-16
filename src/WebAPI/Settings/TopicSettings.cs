namespace WebApi.Settings
{
    public class TopicSettings
    {
        public TopicName KafkaPublisher { get; set; }
        public TopicName KafkaMessagePackPublisher { get; set; }
        public TopicName KafkaHealthCheck { get; set; }

    }
}
