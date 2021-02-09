using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        // As startup file was becoming so bulky so created this extension method and moved some services here.
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // we are registering ProductRepository as service so that we can get it inside controller.
            services.AddScoped<IProductRepository, ProductRepository>();
            // registering generic repository.
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Registering Ibasket as service 
            services.AddScoped<IBasketRepository, BasketRepository>();

            // token service
            services.AddScoped<ITokenService, TokenService>();

             // This is way to handle error list in case of Bad request error.
            services.Configure<ApiBehaviorOptions>(options => {
                    options.InvalidModelStateResponseFactory = actionContext => 
                    {
                        var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .SelectMany(x => x.Value.Errors)
                            .Select(x => x.ErrorMessage).ToArray();
                        
                        var erroResponse = new ApiValidationErrorResponse {
                            Errors = errors
                        };
                        return new BadRequestObjectResult(erroResponse);
                    };
            });


            return services;
        }
    }
}