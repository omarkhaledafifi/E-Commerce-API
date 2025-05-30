using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace Presentation
{
    public class PaymentsController(IServiceManager serviceManager)
        : ApiController
    {
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePayment(string basketId)
        {
            var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await serviceManager.PaymentService.
                UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);

            return new EmptyResult();
        }
    }
}
