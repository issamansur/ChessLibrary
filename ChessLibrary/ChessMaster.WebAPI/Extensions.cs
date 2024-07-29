using System.Text;
using ChessMaster.Application.Services;
using ChessMaster.Infrastructure.Options;
using ChessMaster.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ChessMaster.WebAPI;

public static class Extensions 
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddBearerAuthToSwagger();
        services.AddCors();
        
        return services;
    }
    
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }
    
    // Private methods
    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "MyPolicy",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                );
            }
        );
        
        return services;
    }
    
    private static IServiceCollection AddBearerAuthToSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            (option) =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            }
        );
        
        return services;
    }
}