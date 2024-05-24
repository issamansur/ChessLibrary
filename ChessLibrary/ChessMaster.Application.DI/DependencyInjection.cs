using System.Reflection;
using ChessMaster.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Application.DI;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => 
                cfg.RegisterServicesFromAssemblies(typeof(ITenantFactory).Assembly)
        );
    }
}