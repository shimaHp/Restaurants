

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpadteRestaurantCommandHandler(IRestaurantRepository restaurantRepository
    , ILogger<UpadteRestaurantCommandHandler> logger
    , IMapper mapper
    , IRestaurantAuthorizationService restaurantAuthorizationService

    ) : IRequestHandler<UpadteRestaurantCommand>
    
{
    public async Task Handle(UpadteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating  Restaurant with id:{@RestaurantId} with {@UpdateRestaurant}",request.Id,request);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id);
        if (restaurant is null) 
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Updete))
            throw new ForbidException();
        mapper.Map(request, restaurant);
      
        await restaurantRepository.saveChanges();
      
    }
}
