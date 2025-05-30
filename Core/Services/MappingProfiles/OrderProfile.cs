namespace Services.MappingProfiles
{
    class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<UserAddress, AddressDTO>().ReverseMap();
            CreateMap<OrderAddress, AddressDTO>().ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId
                , options => options.MapFrom(s => s.Product.ProductId))
                 .ForMember(d => d.PictureUrl
                , options => options.MapFrom(s => s.Product.PictureUrl))
                  .ForMember(d => d.ProductName
                , options => options.MapFrom(s => s.Product.ProductName));


            CreateMap<Order, OrderResult>()
                .ForMember(d => d.PaymentStatus,
                options => options.MapFrom(s => s.ToString()))
                 .ForMember(d => d.DeliveryMethod,
                options => options.MapFrom(s => s.DeliveryMethod.ShortName))
                  .ForMember(d => d.Total,
                options => options.MapFrom(s => s.Subtotal + s.DeliveryMethod.Price));

            CreateMap<DeliveryMethod, DeliveryMethodResult>()
                .ForMember(d => d.Cost, options =>
                options.MapFrom(s => s.Price));



        }
    }
}
