namespace ServiceBus.Consumer.Configurations;

public class AzureServiceBusQueuesAndTopicsConfiguration
{
    public required string QueueName { get; set; }
    public required string TopicName { get; set; }
    public required string  SubscriptionName1{ get; set; }
    public required string  SubscriptionName2{ get; set; }
}
