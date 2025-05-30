using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Extensions
{
    public static class PresentationServicesExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(config =>
            {
                config.AddPolicy("CORSPolicy", options =>
                {
                    options.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200");
                });
            });
            return services;
        }
    }
}
