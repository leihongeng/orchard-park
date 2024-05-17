using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.Collections.Generic;

namespace Orchard.Park.Serilog;

public static class WebApplicationBuilderExtension
{
    private const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";

    private static readonly List<LogEventLevel> _normalLevels =
    [
        LogEventLevel.Verbose,
        LogEventLevel.Debug,
        LogEventLevel.Information
    ];

    private static readonly List<LogEventLevel> _warningLevels =
    [
        LogEventLevel.Warning
    ];

    public static IHostBuilder ConfigureSerilogBuilder(this IHostBuilder host, bool isWriteToConsole = true)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Async(writeTo =>
                    writeTo.Logger(lc =>
                        lc.Filter.ByIncludingOnly(e => _normalLevels.Contains(e.Level))
                            .WriteTo.File("./logs/Information-.txt",
                                rollingInterval: RollingInterval.Day,
                                retainedFileCountLimit: 365 * 30,//由于限制了每个文件的大小，按每天生成30个文件算，保留365天
                                outputTemplate: outputTemplate,
                                fileSizeLimitBytes: 25 * 1024 * 1024,//每个文件限制最大25mb
                                rollOnFileSizeLimit: true)
                    )
                )
                .WriteTo.Async(writeTo =>
                    writeTo.Logger(lc =>
                        lc.Filter.ByIncludingOnly(e => _warningLevels.Contains(e.Level))
                            .WriteTo.File("./logs/Warning-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 365, outputTemplate: outputTemplate)
                    )
                )
                .WriteTo.Async(writeTo =>
                    writeTo.Logger(lc =>
                        lc.Filter.ByIncludingOnly(e => !_normalLevels.Contains(e.Level) && !_warningLevels.Contains(e.Level))
                            .WriteTo.File("./logs/Error-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 365, outputTemplate: outputTemplate)
                    )
                );

            if (isWriteToConsole)
            {
                configuration.WriteTo.Async(writeTo => writeTo.Console(outputTemplate: outputTemplate));
            }
            else
            {
                configuration.WriteTo.Async(writeTo =>
                    writeTo.Logger(lc =>
                        lc.Filter.ByIncludingOnly(e => e.Properties.TryGetValue("SourceContext", out var val) && val.ToString().Contains("Microsoft"))
                            .WriteTo.Console(outputTemplate: outputTemplate)
                    )
                );
            }
        });

        return host;
    }
}