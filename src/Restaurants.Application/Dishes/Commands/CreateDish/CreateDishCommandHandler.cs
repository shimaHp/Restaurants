

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(IRestaurantRepository restaurantRepository, IDishesRepository dishesRepository
    , ILogger<CreateDishCommandHandler> logger
    , IMapper mapper
    , IRestaurantAuthorizationService restaurantAuthorizationService
    ) : IRequestHandler<CreateDishCommand,int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new  Dish {@Dish}", request);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
        if(restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Create))
            throw new ForbidException();
        var dish = mapper.Map<Dish>(request);
       return await  dishesRepository.Create(dish);


        //var restaurant = mapper.Map<Dish>(request);
        //int id = await restaurantRepository.Create(Dish);
        //return id;
    }
}
