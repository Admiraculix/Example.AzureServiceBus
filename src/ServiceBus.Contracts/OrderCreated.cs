
namespace ServiceBus.Contracts;

public class OrderCreated
{
    public Guid Id { get; set; }
    public required string ProductName { get; set; }
}
