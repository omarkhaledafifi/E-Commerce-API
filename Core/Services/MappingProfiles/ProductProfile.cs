global using Domain.Entities;

namespace Services.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDTO>()
                .ForMember(d => d.BrandName,
                options => options.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.TypeName,
                options => options.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, options => options.MapFrom<PictureUrlResolver>());


            CreateMap<ProductType, TypeResultDTO>();
            CreateMap<ProductBrand, BrandResultDTO>();

        }
    }
}
