using System.Text;
using ChessMaster.Infrastructure.Data;
using ChessMaster.Infrastructure.Data.Common;
using ChessMaster.Infrastructure.Options;
using ChessMaster.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCQRS();
        services.AddActors();
        services.AddDatabase(configuration);
        services.AddAuth(configuration);
    }
    
    private static void AddCQRS(this IServiceCollection services)
    {
        services.AddScoped<ITenantFactory, TenantFactory>();
    }
    
    private static void AddActors(this IServiceCollection services)
    {
        services.AddSingleton<IActorService, Actors.AkkaNet.AkkaNetSystem>();
    }
    
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStrings = configuration
            .GetSection(nameof(ConnectionStrings))
            .Get<ConnectionStrings>();
        
        services.AddDbContext<ChessMasterDbContext>(options =>
        {
            options.UseNpgsql(connectionStrings?.DefaultConnection);
        });
    }
    
    private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // AuthService (generate JWT)
        services.AddOptions<JwtOptions>()
            .BindConfiguration(nameof(JwtOptions));
        
        services.AddScoped<IAuthService, JwtService>();
        
        // JWT Middleware (validate JWT)
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key)),
                    ValidateIssuerSigningKey = true,
                };
            }
        );
    }
}