namespace ServiceBus.Producer.Requests;
public class CreateCustomerRequest : BaseRequest
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateTime DateOfBirth { get; set; }
}