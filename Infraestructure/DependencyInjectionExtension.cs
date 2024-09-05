using MercadoPagoAPI.Repositories.Contracts;
using MercadoPagoAPI.Services.Contracts;
using MercadoPagoAPI.Services;

namespace MercadoPagoAPI.Infraestructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
    {
        return services
            .AddScoped<IMercadoPagoAPIService, MercadoPagoAPIService>();
    }

    public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services)
    {
        return services
            .AddScoped<IMercadoPagoAPIRepository, MercadoPagoAPIRepository>();
    }

}