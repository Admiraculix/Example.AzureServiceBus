using Azure.Messaging.ServiceBus;

namespace ServiceBus.Producer.Services;

public interface IServiceBusClientWrapper : IAsyncDisposable
{
    IServiceBusSenderWrapper CreateSender(string queueName);
}
