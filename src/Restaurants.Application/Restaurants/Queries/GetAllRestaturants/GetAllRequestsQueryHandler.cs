

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaturants;

public class GetAllRequestsQueryHandler(IRestaurantRepository restaurantRepository, ILogger<GetAllRequestsQueryHandler> logger, IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDTO>>
{
    public async Task<PagedResult<RestaurantDTO>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting all Restaurants");
        var ( restaurants, totalCount )= await restaurantRepository.GetAllMatchingAsync(
            request.SearchPharse
            ,request.PageSize
            ,request.PageNumber
            ,request.SortBy
            ,request.sortDirection);
       
        
        var restaurantsDTOs = mapper.Map<IEnumerable<RestaurantDTO>>(restaurants);
        var result = new PagedResult<RestaurantDTO>(restaurantsDTOs, restaurantsDTOs.Count(), request.PageSize, request.PageNumber);

        return result!;
    }
}
