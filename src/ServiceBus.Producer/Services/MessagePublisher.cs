using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceBus.Producer.Configurations;

namespace ServiceBus.Producer.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly AzureServiceBusQueuesAndTopicsConfiguration _options;

    public MessagePublisher(IServiceProvider serviceProvider, IOptions<AzureServiceBusQueuesAndTopicsConfiguration> options)
    {
        _serviceBusClient = serviceProvider.GetRequiredService<ServiceBusClient>();
        _options = options.Value;
    }

    public Task PublishAsync<T>(T obj, bool isQueue)
    {
        ServiceBusSender sender = _serviceBusClient.CreateSender(isQueue ? _options.QueueName : _options.TopicName);
        var objectAsText = JsonConvert.SerializeObject(obj);
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(objectAsText));
        message.ApplicationProperties["messageType"] = typeof(T).Name;
        return sender.SendMessageAsync(message);
    }

    public Task PublishAsync(string raw, bool isQueue)
    {
        ServiceBusSender sender = _serviceBusClient.CreateSender(isQueue ? _options.QueueName : _options.TopicName));
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(raw));
        message.ApplicationProperties["messageType"] = "Raw";
        return sender.SendMessageAsync(message);
    }
}
