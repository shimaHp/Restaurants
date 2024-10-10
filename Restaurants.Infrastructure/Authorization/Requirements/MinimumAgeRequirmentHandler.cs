

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirmentHandler(ILogger<MinimumAgeRequirmentHandler> logger,IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
        , MinimumAgeRequirment requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User:{Email} dateOf Birth {DoB}- Handeling MinimumAge requerment",

            currentUser.Email,
            currentUser.DateOfBirth);

        if (currentUser.DateOfBirth == null)
        {
            logger.LogWarning("User date of birth is null");
            context.Fail();
            return Task.CompletedTask;

        }
        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
       {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else {
            context.Fail();
        }
        return Task.CompletedTask;

    }
}
