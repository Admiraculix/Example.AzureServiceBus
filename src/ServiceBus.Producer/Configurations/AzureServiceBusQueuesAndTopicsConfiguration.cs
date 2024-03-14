namespace ServiceBus.Producer.Configurations;

public class AzureServiceBusQueuesAndTopicsConfiguration
{
    public required string QueueName { get; set; }
    public required string TopicName { get; set; }
}
