using ChessMaster.Application.DI;
using ChessMaster.Infrastructure;
using ChessMaster.WebAPI;
using ChessMaster.WebAPI.Middlewares.ExceptionHandler;
using Microsoft.OpenApi.Models;

// BUILDER
var builder = WebApplication.CreateBuilder(args);

// Other User Services from other layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Host);

// Configure other Services
builder.Services.ConfigureServices();

// Error Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Autogenerated Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// APP
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.ConfigureApp();

app.MapControllers();

app.Run();