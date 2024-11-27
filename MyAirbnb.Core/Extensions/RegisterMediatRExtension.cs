using Microsoft.Extensions.DependencyInjection;

namespace MyAirbnb.Core.Extensions;

public static class RegisterMediatRExtension
{
    public static void RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    }
}
