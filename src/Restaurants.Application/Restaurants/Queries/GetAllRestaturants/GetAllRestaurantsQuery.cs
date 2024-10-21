

using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaturants;

public class GetAllRestaurantsQuery:IRequest<PagedResult<RestaurantDTO>>
{
    public string? SearchPharse { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public string? SortBy { get; set; }
    public SortDirection sortDirection { get; set; }
}
