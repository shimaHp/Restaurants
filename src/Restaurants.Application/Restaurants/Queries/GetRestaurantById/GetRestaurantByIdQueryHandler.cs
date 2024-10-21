
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(IRestaurantRepository restaurantRepository, ILogger<GetRestaurantByIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDTO?>
{
    public async Task<RestaurantDTO> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting  Restaurant {RestaurantId}",request.Id);
        var retstaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id)
              ??  throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        var restaurantDTO = mapper.Map<RestaurantDTO>(retstaurant);
        return restaurantDTO;
    }
}


