

using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpadteRestaurantCommandValidator:AbstractValidator<UpadteRestaurantCommand>
{
    public UpadteRestaurantCommandValidator()
    {
        RuleFor(c=>c.Name).Length(3,100);
    }
}
