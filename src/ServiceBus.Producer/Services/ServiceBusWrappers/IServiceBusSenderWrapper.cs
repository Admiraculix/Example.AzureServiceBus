using Azure.Messaging.ServiceBus;

namespace ServiceBus.Producer.Services;

public interface IServiceBusSenderWrapper : IAsyncDisposable
{
    Task SendMessageAsync(ServiceBusMessage message);
}