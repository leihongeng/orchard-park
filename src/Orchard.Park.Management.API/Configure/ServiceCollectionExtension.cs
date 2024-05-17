using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Orchard.Park.AspNetCore.Filters;
using Orchard.Park.Core.Converters;
using Orchard.Park.Core.IdGenerator;
using Orchard.Park.Identity;
using Orchard.Park.Model.Configuration;
using Orchard.Park.Service;
using Orchard.Park.SqlSugar;
using SqlSugar;

namespace Orchard.Park.Management.API.Configure;

public static class ServiceCollectionExtension
{
    public const string CorsPolicyName = "Orchard.Park.Management.API";

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static IServiceCollection Build(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrchardParkConfig>(configuration);

        var config = new OrchardParkConfig();
        configuration.Bind(config);

        //初始化雪花ID
        YitIdHelper.Initialize(config.ConnectionStrings.Redis);

        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddControllers(c =>
        {
            c.Filters.Add(typeof(ModelValidFilter));
            c.Filters.Add(typeof(ApiResultFilter));
        })
        .AddNewtonsoftJson(c =>
        {
            c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            c.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            c.SerializerSettings.ContractResolver = new DefaultContractResolver();
            c.SerializerSettings.Converters.Add(new LongToStringConverter());
        })
        .ConfigureApiBehaviorOptions(c =>
        {
            c.SuppressModelStateInvalidFilter = true;
        })
        .AddControllersAsServices();

        //跨域
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, cp => cp.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        });

        //Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orchard.Park.Management.API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            });

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Orchard.Park.Management.API.xml");
            c.IncludeXmlComments(xmlPath, true);
        });
        services.AddSwaggerGenNewtonsoftSupport();

        //Jwt
        services.AddJwtToken(x =>
        {
            configuration.GetSection("TokenOptions").Bind(x);
        });

        //注册SqlSugar
        var connectionList = new List<SqlSugarConnection>
        {
            new()
            {
                ConfigId = 0,
                DbType = DbType.MySqlConnector,
                Connection = config.ConnectionStrings.MySql
            }
        };
        services.AddSqlSugar(connectionList);
        services.AddService();

        return services;
    }
}