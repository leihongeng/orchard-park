using Orchard.Park.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Orchard.Park.AspNetCore.Extensions
{
    public static class InjectionExtension
    {
        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 程序集依赖注入
        /// </summary>
        /// <param name="services">服务实例</param>
        /// <param name="assemblyName">程序集名称。不带DLL</param>
        /// <param name="serviceLifetime">依赖注入的类型 可为空</param>
        /// <param name="suffix">后缀</param>
        /// <param name="ignoreTypes">需要忽略的类型</param>
        public static void AddServiceFromAssembly(this IServiceCollection services,
            string assemblyName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            string suffix = null,
            params Type[] ignoreTypes)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrEmpty(assemblyName)) throw new ArgumentNullException(nameof(assemblyName));

            var assembly = GetAssemblyByName(assemblyName);
            if (assembly == null) throw new DllNotFoundException(nameof(assembly));

            var expression = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract && !o.IsGenericType);
            if (!string.IsNullOrWhiteSpace(suffix))
                expression = expression.Where(x => x.Name.EndsWith(suffix));

            var list = expression.ToList();
            foreach (var type in list)
            {
                var interfacesList = type.GetInterfaces();
                if (!interfacesList.Any())
                    continue;

                var ignoreServiceInjectionAttribute = type.GetCustomAttribute<IgnoreServiceInjectionAttribute>();
                if (ignoreServiceInjectionAttribute != null)
                    continue;

                if (ignoreTypes != null && ignoreTypes.Contains(type))
                    continue;

                foreach (var serviceType in interfacesList)
                {
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(serviceType, type);
                            break;

                        case ServiceLifetime.Scoped:
                            services.AddScoped(serviceType, type);
                            break;

                        case ServiceLifetime.Transient:
                            services.AddTransient(serviceType, type);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}