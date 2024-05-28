using ChessMaster.Infrastructure.Data.Common;
using ChessMaster.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        services.AddScoped<ITenantFactory, TenantFactory>();
        services.AddScoped<IJwtService, JwtService>();
        
        /*
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JwtSettings.ISSUER,
                    
                    ValidateAudience = true,
                    ValidAudience = JwtSettings.AUDIENCE,
                    
                    ValidateLifetime = true,
                    
                    IssuerSigningKey = JwtSettings.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            }
        );
        */
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