namespace ServiceBus.Producer.Requests;

public class CreateOrderRequest : BaseRequest
{
    public Guid Id { get; set; }
    public required string ProductName { get; set; }
}