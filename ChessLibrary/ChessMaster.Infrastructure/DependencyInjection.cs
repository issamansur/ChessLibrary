using ChessMaster.Infrastructure.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantFactory, TenantFactory>();
        
        /*
        services.AddDbContext<ChessMasterDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ChessMasterDbContext).Assembly.FullName)
            )
        );
        */
    }
}