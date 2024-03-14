using System.Diagnostics;
using System.Reflection;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceBus.Consumer.Configurations;
using ServiceBus.Contracts;

namespace ServiceBus.Consumer.Services;

public class MessageSubscriber : IMessageSubscriber
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly AzureServiceBusQueuesAndTopicsConfiguration _options;

    private readonly Dictionary<Type, IList<object>> _modelCollections = new Dictionary<Type, IList<object>>();
    private readonly Dictionary<string, Type> _modelMapping = new Dictionary<string, Type>
        {
            { $"ServiceBus.Contracts.{nameof(CustomerCreated)}", typeof(CustomerCreated) },
            { $"ServiceBus.Contracts.{nameof(OrderCreated)}", typeof(OrderCreated) },
        };

    public MessageSubscriber(IServiceProvider serviceProvider, IOptions<AzureServiceBusQueuesAndTopicsConfiguration> options)
    {
        _serviceBusClient = serviceProvider.GetRequiredService<ServiceBusClient>();
        _options = options.Value;
    }

    public IList<CustomerCreated> CustomerCreatedCollection { get; private set; } = new List<CustomerCreated>();
    public IList<OrderCreated> OrderCreatedCollection { get; private set; } = new List<OrderCreated>();

    public async Task SubscribeToQueueAsync()
    {
        ServiceBusReceiver receiver = _serviceBusClient.CreateReceiver(_options.QueueName);
        IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);

        if (receivedMessages.Count == 0)
        {
            return;
        }

        foreach (ServiceBusReceivedMessage receivedMessage in receivedMessages)
        {
            try
            {
                object messageType = GetMessageTypeName(receivedMessage);

                if (_modelMapping.TryGetValue(messageType.ToString(), out Type modelType))
                {
                    AddToSpecificCollection(messageType);
                    await receiver.CompleteMessageAsync(receivedMessage);
                }
                else
                {
                    Debug.WriteLine($"Unknown message type: {messageType}");
                }

            }
            catch (Exception ex)
            {
                await receiver.AbandonMessageAsync(receivedMessage);
                Debug.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        CustomerCreatedCollection = GetModels<CustomerCreated>();
        OrderCreatedCollection = GetModels<OrderCreated>();
    }

    public async Task SubscribeToTopicSubscriptionCustomerAsync()
    {
        ServiceBusReceiver receiver = _serviceBusClient.CreateReceiver(_options.TopicName, _options.SubscriptionName2);
        IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);

        if (receivedMessages.Count == 0)
        {
            return;
        }

        foreach (ServiceBusReceivedMessage receivedMessage in receivedMessages)
        {
            try
            {
                var customerCreated = JsonConvert.DeserializeObject<CustomerCreated>(Encoding.UTF8.GetString(receivedMessage.Body));
                CustomerCreatedCollection.Add(customerCreated);

                await receiver.CompleteMessageAsync(receivedMessage);
            }
            catch (Exception ex)
            {
                await receiver.AbandonMessageAsync(receivedMessage);
                Debug.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }

    public async Task SubscribeToTopicSubscriptionOrderAsync()
    {
        ServiceBusReceiver receiver = _serviceBusClient.CreateReceiver(_options.TopicName, _options.SubscriptionName1);
        IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);


        if (receivedMessages.Count == 0)
        {
            return;
        }

        foreach (ServiceBusReceivedMessage receivedMessage in receivedMessages)
        {
            try
            {
                var orderCreated = JsonConvert.DeserializeObject<OrderCreated>(Encoding.UTF8.GetString(receivedMessage.Body));
                OrderCreatedCollection.Add(orderCreated);

                await receiver.CompleteMessageAsync(receivedMessage);
            }
            catch (Exception ex)
            {
                await receiver.AbandonMessageAsync(receivedMessage);
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }

    private object GetMessageTypeName(ServiceBusReceivedMessage receivedMessage)
    {
        string json = Encoding.UTF8.GetString(receivedMessage.Body);
        string? stringAsObjectType = receivedMessage?.ApplicationProperties["messageType"].ToString();
        object deserializedObject = DeserializeJsonToObject(stringAsObjectType, json);

        return deserializedObject;
    }

    // Method to add deserialized model to specific collection based on its type
    private void AddToSpecificCollection(object model)
    {
        if (!_modelCollections.TryGetValue(model.GetType(), out IList<object>? collection))
        {
            collection = new List<object>();
            _modelCollections[model.GetType()] = collection;
        }

        collection.Add(model);
    }

    // Method to get models of specific type from the collection
    private List<T> GetModels<T>()
    {
        if (_modelCollections.TryGetValue(typeof(T), out IList<object>? collection))
        {
            return collection.OfType<T>().ToList();
        }

        return new List<T>();
    }

    private object DeserializeJsonToObject(string objectType, string jsonString)
    {
        Assembly assm = Assembly.Load("ServiceBus.Contracts");
        Type? type = assm.GetType($"ServiceBus.Contracts.{objectType}");
        MethodInfo? deserializeMethod = typeof(JsonConvert).GetMethod("DeserializeObject", new[] { typeof(string), typeof(Type) });
        return deserializeMethod.Invoke(null, [jsonString, type]);
    }
}
