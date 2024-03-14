using Microsoft.AspNetCore.Mvc;
using ServiceBus.Consumer.Services;

namespace ServiceBus.Consumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumerController : ControllerBase
    {

        private readonly ILogger<ConsumerController> _logger;
        private readonly IMessageSubscriber _messageSubscriber;

        public ConsumerController(ILogger<ConsumerController> logger, IMessageSubscriber messageSubscriber)
        {
            _logger = logger;
            _messageSubscriber = messageSubscriber;
        }

        [HttpGet(Name = "GetConsumerData")]
        public async Task<IActionResult> GetAsync()
        {
            await _messageSubscriber.SubscribeToQueueAsync();
            await _messageSubscriber.SubscribeToTopicSubscriptionCustomerAsync();
            await _messageSubscriber.SubscribeToTopicSubscriptionOrderAsync();

            var ordersMsg = _messageSubscriber.OrderCreatedCollection?.Count;
            var customersMsg = _messageSubscriber.CustomerCreatedCollection?.Count;

            return Ok(new { OrderCount = ordersMsg, CustomerCount = customersMsg});
        }
    }
}
