using Microsoft.Extensions.Configuration;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductResultDTO, string>
    {
        public string Resolve(Product source, ProductResultDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.PictureUrl)) return string.Empty;

            return $"{configuration["BaseUrl"]}{source.PictureUrl}";


        }
    }
}
/// BaseUrl/images/products/CheesyVegetableLasagna.png,