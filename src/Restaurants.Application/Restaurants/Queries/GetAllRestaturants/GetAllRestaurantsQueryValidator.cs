

using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaturants;

public class GetAllRestaurantsQueryValidator :AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSize = [5, 10, 15, 30];
    private string[] allowSortByColumnNames = [
        nameof(RestaurantDTO.Name),
        nameof(RestaurantDTO.Category),
        nameof(RestaurantDTO.Description)
        ];
    public GetAllRestaurantsQueryValidator() 
    {
        RuleFor(r=>r.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSize.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",",allowPageSize)}]");

        RuleFor(r => r.SortBy)
            .Must(value => allowSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optinal,or must be in [{string.Join(",", allowSortByColumnNames)}]");

    }
}
