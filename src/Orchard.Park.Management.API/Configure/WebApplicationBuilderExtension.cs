using Orchard.Park.Serilog;

namespace Orchard.Park.Management.API.Configure;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureSerilogBuilder(this WebApplicationBuilder builder)
    {
        builder.Host.ConfigureSerilogBuilder(isWriteToConsole: !builder.Environment.IsProduction());
        return builder;
    }
}