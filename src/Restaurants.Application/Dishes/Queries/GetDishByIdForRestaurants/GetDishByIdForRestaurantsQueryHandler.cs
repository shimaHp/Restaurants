using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Dishes.Queries.GetAllForRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurants
{
    internal class GetDishByIdForRestaurantsQueryHandler(
        IRestaurantRepository restaurantRepository,
        ILogger<GetDishByIdForRestaurantsQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantsQuery, DishDTO>
    {
        public async Task<DishDTO> Handle(GetDishByIdForRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting  Dishe for retaurant id {request.RestaurantId} and dish id={request.DishId}");
            var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var dish=restaurant.Dishes.FirstOrDefault(d=>d.Id==request.DishId);
            if(dish==null) throw new NotFoundException(nameof(dish), request.DishId.ToString());
            var results = mapper.Map<DishDTO>(dish);
            return results;
        }
    }
}
