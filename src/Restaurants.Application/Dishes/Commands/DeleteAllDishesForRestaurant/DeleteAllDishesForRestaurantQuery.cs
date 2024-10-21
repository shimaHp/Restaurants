

using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;

public class DeleteAllDishesForRestaurantQuery(int restaurantId):IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
