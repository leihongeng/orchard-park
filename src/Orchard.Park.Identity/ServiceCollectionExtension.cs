using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Orchard.Park.Identity
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtToken(this IServiceCollection services, Action<JwtTokenConfig> acConfig, JwtBearerEvents events = null)
        {
            if (acConfig == null)
                throw new ArgumentNullException($"请添加{nameof(acConfig)}配置");

            var config = new JwtTokenConfig();
            acConfig.Invoke(config);

            services.AddSingleton(config);

            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<ICurrentUser, CurrentUser>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否在令牌期间验证签发者
                        ValidateAudience = true,//是否验证接收者
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证签名
                        ValidAudience = config.Audience,//接收者
                        ValidIssuer = config.Issuer,//签发者，签发的Token的人
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey))
                    };

                    if (events != null)
                        options.Events = events;
                });

            return services;
        }
    }
}