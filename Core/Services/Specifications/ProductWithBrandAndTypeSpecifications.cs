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


        public ProductWithBrandAndTypeSpecifications()
           : base(null)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);



        }
    }
}
