namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : Specifications<Product>
    {
        // use for Retrieving One Product 
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(product => product.Id == id)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }


        public ProductWithBrandAndTypeSpecifications(ProductSpecificationsParameters parameters)
           : base(product =>
           (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) &&
           (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId) &&
           (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim())))
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);


            ApplyPagination(parameters.pageIndex, parameters.PageSize);
            if (parameters.Sort is not null)
            {
                switch (parameters.Sort)
                {
                    case ProductSortingOptions.NameDesc:
                        SetOrderByDescending(product => product.Name);
                        break;
                    case ProductSortingOptions.NameAsc:
                        SetOrderBy(product => product.Name);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        SetOrderByDescending(product => product.Price);
                        break;
                    case ProductSortingOptions.PriceAsc:
                        SetOrderBy(product => product.Price);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
