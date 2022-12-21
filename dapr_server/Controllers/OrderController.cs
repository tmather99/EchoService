using Dapr;
using GloboTicket.Ordering.Model;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.Ordering.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> logger;

    public OrderController(ILogger<OrderController> logger)
    {
        this.logger = logger;
    }

    [HttpPost("", Name = "SubmitOrder")]
    [Topic("pubsub", "orders")]
    public IActionResult Submit(OrderForCreation order)
    { 
        logger.LogInformation($"Received a new order from {order.CustomerDetails.Name}");
        return Ok(order);
    }
}
