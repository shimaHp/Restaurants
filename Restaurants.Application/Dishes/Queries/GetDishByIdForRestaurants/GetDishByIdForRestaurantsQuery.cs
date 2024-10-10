
using MediatR;
using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurants;

public class GetDishByIdForRestaurantsQuery(int restaurantId,int dishId):IRequest<DishDTO> 
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}

