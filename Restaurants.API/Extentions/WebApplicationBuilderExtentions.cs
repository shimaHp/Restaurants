using Microsoft.OpenApi.Models;
using Restaurants.API.Middelwares;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extension;
using Serilog;

namespace Restaurants.API.Extentions;

public static class WebApplicationBuilderExtentions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {

            Reference= new OpenApiReference{Type = ReferenceType.SecurityScheme,Id="bearerAuth"}
        },
        []
      }
    });
        });
        builder.Services.AddScoped<ErrorHandelingMiddleware>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    }
}
