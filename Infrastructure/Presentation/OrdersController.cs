using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.OrderModels;
using System.Security.Claims;

namespace Presentation
{
    [Authorize]
    public class OrdersController(IServiceManager serviceManager)
        : ApiController
    {

        [HttpPost] // Post : api/Orders 
        public async Task<ActionResult<OrderResult>> Create(OrderRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.OrderService.CreateOrderAsync(request, email);

            return Ok(order);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManager.OrderService.GetOrdersByEmailAsync(email);

            return Ok(orders);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResult>> GetOrder(Guid id)
        {
            var orders = await serviceManager.OrderService.GetOrderByIdAsync(id);

            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethodResult>> GetDeliveryMethods()
        {
            var methods = await serviceManager.OrderService.GetDeliveryMethodsAsync();

            return Ok(methods);
        }

    }
}
