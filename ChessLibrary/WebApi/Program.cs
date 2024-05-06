using ChessMaster.Application.Common;
using ChessMaster.Infrastructure.Data.Common;
using ChessMaster.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChessMasterDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Add services to the container.

builder.Services.AddScoped<ITenantFactory, TenantFactory>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddMediatR(
    cfg => 
        cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();