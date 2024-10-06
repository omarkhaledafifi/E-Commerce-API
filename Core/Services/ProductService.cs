global using AutoMapper;
global using Domain.Contracts;
global using Services.Abstractions;
global using Shared;
using Domain.Exceptions;
using Services.Specifications;

namespace Services
{
    // Primary Constructor C# 12.0 Feature 
    class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsResult = mapper.Map<IEnumerable<BrandResultDTO>>(brands);
            return brandsResult;
        }

        public async Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpecificationsParameters parameters)
        {
            var products = await unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(new ProductWithBrandAndTypeSpecifications(parameters));
            var productsResult = mapper.Map<IEnumerable<ProductResultDTO>>(products);
            var count = productsResult.Count();

            var totalCount = await unitOfWork.GetRepository<Product, int>()
                .CountAsync(new ProductCountSpecifications(parameters));

            var result = new PaginatedResult<ProductResultDTO>
                (parameters.pageIndex,
                count,
                    totalCount,
                    productsResult);

            return result;
        }

        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typesResult = mapper.Map<IEnumerable<TypeResultDTO>>(types);
            return typesResult;
        }

        public async Task<ProductResultDTO?> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(new ProductWithBrandAndTypeSpecifications(id));
            return product is null ? throw new ProductNotFoundException(id)
                : mapper.Map<ProductResultDTO?>(product);
        }
    }
}
