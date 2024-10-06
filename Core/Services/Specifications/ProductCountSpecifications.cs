namespace Services.Specifications
{
    internal class ProductCountSpecifications : Specifications<Product>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters)
           : base(product =>
           (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) &&
           (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId) &&
           (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim())))
        {
        }
    }
}
