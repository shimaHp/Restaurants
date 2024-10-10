

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository
    , ILogger<CreateRestaurantCommandHandler> logger
    , IMapper mapper,IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleteing  Restaurant with id:{RestaurantId}",request.Id);
            var retstaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id);
            if (retstaurant is null)
                throw new NotFoundException(nameof(Restaurant),request.Id.ToString());

            if (!restaurantAuthorizationService.Authorize(retstaurant, Domain.Constants.ResourceOperation.Delete))
                throw new ForbidException();
                await restaurantRepository.Delete(retstaurant);
           
        }
    }
}
