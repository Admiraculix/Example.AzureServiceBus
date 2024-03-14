using Azure.Messaging.ServiceBus;

namespace ServiceBus.Producer.Services;

public class ServiceBusSenderWrapper : IServiceBusSenderWrapper
{
    private readonly ServiceBusSender _serviceBusSender;
    private readonly IServiceBusClientWrapper _serviceBusClientWrapper;

    public ServiceBusSenderWrapper(IServiceBusClientWrapper serviceBusClientWrapper)
    {
        _serviceBusClientWrapper = serviceBusClientWrapper;
    }

    public Task SendMessageAsync(ServiceBusMessage message)
    {
        return _serviceBusSender.SendMessageAsync(message);
    }

    public ValueTask DisposeAsync()
    {
        return _serviceBusSender.DisposeAsync();
    }
}
