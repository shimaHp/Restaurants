using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;
using System;


namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validateCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
                .Length(3, 100);
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is requerd.");

        RuleFor(dto => dto.Category)
            .Must(validateCategories.Contains)
            .WithMessage("Invalid categoey. Please choose from the valid categoeris.");
        //.Custom((value, context) => { 
        //var isvalidateCatgoery= validateCategories.Contains(value);
        //    if(!isvalidateCatgoery)
        //    {
        //        context.AddFailure("Categoery", "Invalid categoey. Please choose from the valid categoeris.");
        //    }


        //});

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("please provid a valid email address");
        RuleFor(dto => dto.PotCode)
     .Matches(@"^[A-Z]{1,2}\d{1,2} \d[A-Z]{2}$")
     .WithMessage("Please add correct PostCode");



    }
}
