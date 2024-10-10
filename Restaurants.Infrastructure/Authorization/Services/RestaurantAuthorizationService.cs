

using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext) : IRestaurantAuthorizationService

{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorixing user {UserEmail},to {Operation} for restaurant {RestaurantName}"
            , user.Email, resourceOperation, restaurant.Name);
        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read Operation - successful authorizaition");
            return true;
        }
        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user , Delete OPeration  - successful authorizaition");
            return true;
        }
        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Updete) && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant Owner  - successful authorizaition");
            return true;
        }
        return false;
    }

   
}
