using Azure.Messaging.ServiceBus;

namespace ServiceBus.Producer.Services;

public class ServiceBusClientWrapper : IServiceBusClientWrapper
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly IServiceProvider _serviceProvider;

    public ServiceBusClientWrapper(IServiceProvider serviceProvider)
    {
        _serviceBusClient = serviceProvider.GetRequiredService<ServiceBusClient>();
        _serviceProvider = serviceProvider;
    }

    public IServiceBusSenderWrapper CreateSender(string queueName)
    {
        ServiceBusSender sender = _serviceBusClient.CreateSender(queueName);
        return new ServiceBusSenderWrapper(this);
    }

    public ValueTask DisposeAsync()
    {
        return _serviceBusClient.DisposeAsync();
    }
}
