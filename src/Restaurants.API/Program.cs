
using Microsoft.OpenApi.Models;
using Restaurants.API.Extentions;
using Restaurants.API.Middelwares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extension;
using Restaurants.Infrastructure.Seesders;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantsSeeder>();
await seeder.seed();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandelingMiddleware>();
app.UseSerilogRequestLogging();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
