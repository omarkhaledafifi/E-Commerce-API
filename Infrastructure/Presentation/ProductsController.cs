using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System.Net;
namespace Presentation
{
    public class ProductsController(IServiceManager ServiceManager) : ApiController
    {
        [RedisCache(120)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDTO>>> GetAllProducts([FromQuery] ProductSpecificationsParameters parameters)
        {

            var products = await ServiceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(products);
        }

        [ProducesResponseType(typeof(ProductResultDTO), (int)HttpStatusCode.OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResultDTO>> GetProduct(int id)
        {
            var product = await ServiceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetAllBrands()
        {
            var brands = await ServiceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeResultDTO>>> GetAllTypes()
        {
            var types = await ServiceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
