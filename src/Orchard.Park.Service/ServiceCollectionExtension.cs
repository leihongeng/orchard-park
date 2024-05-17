using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orchard.Park.AspNetCore.Extensions;
using Orchard.Park.IRepository.Infrastructure;
using Orchard.Park.Repository.Infrastructure;

namespace Orchard.Park.Service;

public static class ServiceCollectionExtension
{
    public static void AddService(this IServiceCollection services)
    {
        services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        services.AddServiceFromAssembly("Orchard.Park.Repository", suffix: "Repository");
        services.AddServiceFromAssembly("Orchard.Park.Service", suffix: "Service");
    }
}