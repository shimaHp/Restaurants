

using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequerment(int minimumRetaurantsCreated) :IAuthorizationRequirement
{
    public int MinimumRetaurantsCreated { get; } = minimumRetaurantsCreated;
}
