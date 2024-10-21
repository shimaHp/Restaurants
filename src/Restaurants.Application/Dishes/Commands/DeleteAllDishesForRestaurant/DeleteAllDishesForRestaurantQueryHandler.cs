
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;

public class DeleteAllDishesForRestaurantQueryHandler(ILogger<DeleteAllDishesForRestaurantQueryHandler> logger
    , IMapper mapper
    , IRestaurantRepository restaurantRepository
    ,IDishesRepository dishesRepository
    ,IRestaurantAuthorizationService restaurantAuthorizationService
    ) : IRequestHandler<DeleteAllDishesForRestaurantQuery>
{
    public async Task Handle(DeleteAllDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Delete all  Dishes for Restaurants{Restaurant}", request.RestaurantId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
        if (restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        if( restaurant.Dishes.Any())
            throw new NotFoundException(nameof(Dishes), request.RestaurantId.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Updete))
            throw new ForbidException();
        await  dishesRepository.Delete(restaurant.Dishes);

    }
}
