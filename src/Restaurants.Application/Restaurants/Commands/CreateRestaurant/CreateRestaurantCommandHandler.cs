﻿

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantRepository restaurantRepository
    , ILogger<CreateRestaurantCommandHandler> logger
    , IMapper mapper,
    IUserContext userContext
    ) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser=userContext.GetCurrentUser();   
        logger.LogInformation("{UserEmail} [{UserId}] is Creating a new  Restaurants {@Restaurant}"
            ,currentUser.Email,
            currentUser.Id,
            request);
        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId=currentUser.Id;
        int id = await restaurantRepository.Create(restaurant);
        return id;
    }
}
