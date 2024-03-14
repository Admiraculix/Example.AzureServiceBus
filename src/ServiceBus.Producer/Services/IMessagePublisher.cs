namespace ServiceBus.Producer.Services;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T obj, bool isQueue);
    Task PublishAsync(string raw, bool isQueue);
}