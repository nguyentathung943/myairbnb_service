using Serilog;

namespace MyAirbnb.API.Extensions;

public static class LoggerConfigurationExtension
{
    public static void ConfigureApplicationLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));
    }
}
