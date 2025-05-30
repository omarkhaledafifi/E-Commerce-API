namespace Services
{
    public sealed class ServiceManager(IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository,
        UserManager<User> userManager,
        IOptions<JwtOptions> options,
        IConfiguration configuration,
        ICacheRepository cacheRepository
            ) : IServiceManager
    {
        private readonly Lazy<IProductService> _productService = new(() => new ProductService(unitOfWork, mapper));
        private readonly Lazy<IBasketService> _lazyBasketService =
            new(() => new BasketService(basketRepository, mapper));
        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService =
            new(() => new AuthenticationService(userManager, options, mapper));
        private readonly Lazy<IOrderService> _lazyOrderService =
            new(() => new OrderService(unitOfWork, mapper, basketRepository));
        private readonly Lazy<IPaymentService> _lazyPaymentService =
                new(() => new PaymentService(basketRepository, unitOfWork, configuration, mapper));

        private readonly Lazy<ICacheService> _lazyCacheService =
            new(() => new CacheService(cacheRepository));

        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _lazyBasketService.Value;

        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;

        public IOrderService OrderService => _lazyOrderService.Value;

        public IPaymentService PaymentService => _lazyPaymentService.Value;

        public ICacheService CacheService => _lazyCacheService.Value;
    }
}