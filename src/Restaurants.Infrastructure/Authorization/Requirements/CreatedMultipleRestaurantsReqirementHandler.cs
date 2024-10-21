
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements
{

    internal class CreatedMultipleRestaurantsReqirementHandler(IRestaurantRepository restaurantRepository, IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequerment>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequerment requirement)
        {
            var currentUser = userContext.GetCurrentUser();
            var restaureants = await restaurantRepository.GetAllAsync();
            var userRestaurantsCreated = restaureants.Count(r => r.OwnerId == currentUser!.Id);
            if (userRestaurantsCreated >= requirement.MinimumRetaurantsCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

        }
    }
}
