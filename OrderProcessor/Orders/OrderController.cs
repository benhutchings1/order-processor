using Microsoft.AspNetCore.Mvc;
using OrderProcessor.Models;
using OrderProcessor.Orders;

namespace OrderProcessor.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpPost()]
    async public Task CreateOrder(IOrderService orderService, Order newOrder)
    {
        await orderService.SubmitOrder(newOrder);
    }

}
