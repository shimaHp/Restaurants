using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

using Restaurants.Infrastructure.Persistence;
using System.ComponentModel;

namespace Restaurants.Infrastructure.Seesders;


internal class RestaurantsSeeder(RestaurantsDbContext dbContext) : IRestaurantsSeeder
{
    public async Task seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurant = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurant);
                await dbContext.SaveChangesAsync();
            }
            if(!dbContext.Roles.Any())
            {
                var roles= GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            
            new (UserRoles.User)
            {
                NormalizedName= UserRoles.User.ToUpper(),
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper(),
            },
            new (UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpper(),
            },
        ];
        return roles; 
    }
    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [new()
        {
            Name = "KFC",
            Category = "fast food",
            Description = "KFC(short for kentucy fried cheken ) is an American fast food restaurants",
            ContactEmail = "contact@kfc.com",
            HasDelivery = true,
            Dishes = [
                new()
                {
                    Name = "Nashvile Hot chicken",
                    Description = "Nashvil Hot chicken (10 pcs)",
                    Price = 10.30M
                },
                new()
                {
                    Name = "Nashvile Cold chicken",
                    Description = "Nashvil cold chicken (5 pcs)",
                    Price = 5.30M
                },

            ]
        },
            new Restaurant()
            {
                Name = "McDonald",
                Category = "fast food",
                Description = "Corporation",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 195",
                    PotCode = "WLF 8SR"
                }
            }


        ];
        return restaurants;
    }
}
