using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Orchard.Park.AspNetCore.Middlewares;
using Orchard.Park.Identity;
using Orchard.Park.Management.API.Configure;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Runtime.InteropServices;
using ServiceCollectionExtension = Orchard.Park.Management.API.Configure.ServiceCollectionExtension;

namespace Orchard.Park.Management.API;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Async(writeTo => writeTo.Console())
            .CreateBootstrapLogger();

        Log.Information("Starting up!");
        Log.Information($"Version:{Assembly.GetEntryAssembly()?.GetName().Version}");
        Log.Information($"RuntimeIdentifier:{RuntimeInformation.RuntimeIdentifier}");
        Log.Information($"Framework Version:{RuntimeInformation.FrameworkDescription}");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Build(builder.Configuration);

            //������־
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.CombineLogs = true;
                logging.ResponseBodyLogLimit = 96 * 1024;
            });

            var app = builder.Build();

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            app.UseRouting();

            //����
            app.UseCors(ServiceCollectionExtension.CorsPolicyName);

            //�����֤
            app.UseAuthentication();
            app.UseAuthorization();

            //�����������������־
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpLogging();
            }

            //ȫ���쳣
            app.UseExceptionHandlerMiddleWare(false, context =>
            {
                var userId = context.RequestServices.GetRequiredService<ICurrentUser>().GetUserIdOrDefault();
                return string.IsNullOrWhiteSpace(userId) ? "[UserId:NotLogin]" : $"[UserId:{userId}]";
            });

            //�����������Swagger�ĵ�
            if (!app.Environment.IsProduction())
            {
                app.Map("/", async httpContext =>
                {
                    await httpContext.Response.WriteAsync(
                        "<div style=\"text-align:center\"><h1>Orchard.Park.Management.API started successfully</h1><a href=\"swagger\">Swagger</a></div>");
                });
                app.MapSwagger();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Orchard.Park.Management.API V1");
                    c.DocExpansion(DocExpansion.None); //Ĭ���۵�
                    c.EnableFilter();
                });
            }

            app.MapControllers();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "An unhandled exception occured during bootstrapping");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}