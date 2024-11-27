using Microsoft.EntityFrameworkCore;
using MyAirbnb.API.Middlewares;
using MyAirbnb.DataAccess;

namespace MyAirbnb.API.Extensions;

public static class RegisterServicesExtension
{
    public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Application DB Context with Connection String
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Configure Global Exception Middleware
        services.AddTransient<GlobalExceptionMiddleware>();
    }
}
