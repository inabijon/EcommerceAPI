using Core.Interfaces;
using EcommerceAPI.Errors;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Extensions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                    .Where(e => e.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors)
                                    .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidation
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            
            return services;
        }
    }
}