using Shared;

namespace Services.Abstractions
{
    public interface IProductService
    {
        // Get All Products 
        public Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpecificationsParameters parameters);
        // Get  Product By Id 
        public Task<ProductResultDTO?> GetProductByIdAsync(int id);

        // Get All Types 
        public Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync();
        // Get All Brands
        public Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync();
    }
}
