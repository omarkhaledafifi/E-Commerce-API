using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager serviceManager)
        : ControllerBase
    {
        [HttpGet] // Get baseUrl/api/Basket?value
        public async Task<ActionResult<BasketDTO>> Get(string id)
        {
            var basket = await serviceManager.BasketService.GetBasketAsync(id);
            return Ok(basket);
        }


        [HttpPost]// Post  baseUrl/api/Basket
        public async Task<ActionResult<BasketDTO>> Update(BasketDTO basketDTO)
        {
            var basket = await serviceManager.BasketService.UpdateBasketAsync(basketDTO);
            return Ok(basket);
        }


        [HttpDelete("{id}")] // Delete baseUrl/api/Basket/value
        public async Task<ActionResult> Delete(string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent(); // 204
        }
    }
}
