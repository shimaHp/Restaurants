using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seesders;


namespace Restaurants.Infrastructure.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services,IConfiguration configuration) {

        var connectionstring = configuration.GetConnectionString("RestaurantsDB");

        services.AddDbContext<RestaurantsDbContext>(options =>
        options.UseSqlServer(connectionstring)
        .EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();
        services.AddScoped<IRestaurantsSeeder, RestaurantsSeeder>();
        services.AddScoped<IRestaurantRepository, RestaurantsRepositry>();
        services.AddScoped<IDishesRepository,DishesRepository>();
        services.AddAuthorizationBuilder()

            .AddPolicy(PolicyNames.AtLeast20,
                 builder => builder.AddRequirements(new MinimumAgeRequirment(20)))
          .AddPolicy(PolicyNames.CreatedAtLeast2Restaurants,
                 builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequerment(2)))
            .AddPolicy(PolicyNames.HasNationaHlity,
                 builder => builder.RequireClaim(AppClaimTypes.Nationality,"Germany","Polish"));


        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();
        services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsReqirementHandler>();
        services.AddScoped<IRestaurantAuthorizationService,RestaurantAuthorizationService>();

    }
}
