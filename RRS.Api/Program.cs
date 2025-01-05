using RRS.Api.Extensions;
using RRS.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .AddSwagger()
    .AddCorsPolicy()
    .AddBearerAuthentication()
    .AddApiServices()
    .AddApplicationLayer()
    .AddInfrastructureLayer();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();

    using (var scope = app.Services.CreateScope()) 
    {
        var dataInitializer = scope.ServiceProvider.GetRequiredService<DataInitializer>(); 
        await dataInitializer.SeedRolesAndUsersAsync(); 
    }
}


app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
