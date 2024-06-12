using Microsoft.AspNetCore.Mvc;
using ServiceBus.Contracts;
using ServiceBus.Producer.Requests;
using ServiceBus.Producer.Services;

namespace ServiceBus.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly ILogger<MessagingController> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public MessagingController(ILogger<MessagingController> logger, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        //use this endpoint from Postman
        [HttpPost("publish/text")]
        [Consumes("text/plain")]
        [Produces("text/plain")]
        public async Task<IActionResult> PublishTextAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var bodyAsText = await reader.ReadToEndAsync();
            await _messagePublisher.PublishAsync(bodyAsText, true);
            return Ok();
        }

        [HttpPost("publish/customer")]
        public async Task<IActionResult> PublishCustomerAsync([FromBody] CreateCustomerRequest request)
        {
            var customerCreated = new CustomerCreated
            {
                Id = request.Id,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth
            };

            await _messagePublisher.PublishAsync(customerCreated, request.IsQueue);
            return Ok();
        }

        [HttpPost("publish/order")]
        public async Task<IActionResult> PublishOrderAsync([FromBody] CreateOrderRequest request)
        {
            var orderCreated = new OrderCreated
            {
                Id = request.Id,
                ProductName = request.ProductName,
            };

            await _messagePublisher.PublishAsync(orderCreated, request.IsQueue);
            return Ok();
        }
    }
}
