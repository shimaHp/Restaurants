

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Restaurants.Queries.GetAllRestaturants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetAllForRestaurant
{
    public class GetDishesForRestaurantQueryHandler(
        IRestaurantRepository restaurantRepository,
        ILogger<GetDishesForRestaurantQueryHandler> logger,
        IMapper mapper) 
        : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDTO>>
    {
        public async Task<IEnumerable<DishDTO>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting all Dishes for retaurant id {request.RestaurantId}");
            var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant == null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
            var results=mapper.Map<IEnumerable<DishDTO>>(restaurant.Dishes);
            return results;

        }
    }
}
