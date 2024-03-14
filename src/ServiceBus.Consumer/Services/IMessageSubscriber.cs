using ServiceBus.Contracts;

namespace ServiceBus.Consumer.Services;

public interface IMessageSubscriber
{
    Task SubscribeToQueueAsync();
    Task SubscribeToTopicSubscriptionCustomerAsync();
    Task SubscribeToTopicSubscriptionOrderAsync();
    IList<CustomerCreated> CustomerCreatedCollection { get; }
    IList<OrderCreated> OrderCreatedCollection { get;  }
}
