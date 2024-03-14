namespace ServiceBus.Consumer.Services.BackgroundJobs;

public class CustomerConsumerService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;

    public CustomerConsumerService(IMessageSubscriber messageSubscriber)
    {
        _messageSubscriber = messageSubscriber;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //TODO: implement some logic
        //await _messageSubscriber.SubscribeToQueueAsync();
        //var ordersMsg = _messageSubscriber.OrderCreatedCollection?.Count;
        //var customersMsg = _messageSubscriber.CustomerCreatedCollection?.Count;
    }
}
