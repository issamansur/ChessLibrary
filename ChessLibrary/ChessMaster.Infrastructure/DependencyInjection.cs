using System.Text;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;
using ChessMaster.Infrastructure.Data;
using ChessMaster.Infrastructure.Data.Common;
using ChessMaster.Infrastructure.Options;
using ChessMaster.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder)
    {
        services.AddCQRS();
        services.AddActorMicrosoftOrleans(hostBuilder);
        services.AddDatabase(configuration);
        services.AddAuth(configuration);
    }
    
    private static void AddCQRS(this IServiceCollection services)
    {
        services.AddScoped<ITenantFactory, TenantFactory>();
    }
    
    private static void AddActorAkkaNet(this IServiceCollection services)
    {
        services.AddSingleton<IChessActorService, Actors.AkkaNet.AkkaNetSystem>();
        // starts the IHostedService, which creates the ActorSystem and actors
        services.AddHostedService<Actors.AkkaNet.AkkaNetSystem>(sp => (Actors.AkkaNet.AkkaNetSystem)sp.GetRequiredService<IChessActorService>());
    }
    
    private static void AddActorMicrosoftOrleans(this IServiceCollection services, IHostBuilder hostBuilder)
    {
        hostBuilder.UseOrleans(siloBuilder =>
        {
            siloBuilder
                .UseLocalhostClustering();
        });

        //services.AddGrainService<Actors.MicrosoftOrleans.MicrosoftOrleansSystem>();
        services.AddSingleton<IChessActorService, Actors.MicrosoftOrleans.MicrosoftOrleansSystem>();
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