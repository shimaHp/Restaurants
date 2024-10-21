using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validatore_forValidCommand_ShouldNotHaveValidatorErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Description = "Test",
            Category = "Italian",
            ContactEmail  = "test@mail.com",
            PotCode= "B86 1GY"
        };
        var validator= new CreateRestaurantCommandValidator();
        //act
        var result=validator.TestValidate(command);
        
        //assert
        result.ShouldNotHaveAnyValidationErrors();  
    }
    [Fact()]
    public void Validatore_forValidCommand_ShouldHaveValidatorErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Description = "Ita",
            Category = "Ital",
            ContactEmail = "@mail.com",
            PotCode = "B661GY"
        };
        var validator = new CreateRestaurantCommandValidator();
        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldHaveValidationErrorFor(c=>c.Name);
        result.ShouldHaveValidationErrorFor(c=>c.Category);
        result.ShouldHaveValidationErrorFor(c=>c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c=>c.PotCode);
    }
    //"Italian", "Mexican", "Japanese", "American", "Indian"
    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoeryProperty(string categoery)
    {
        //arrange
        var validatore = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { Category = categoery };
        //act
        var result = validatore.TestValidate(command);
        //assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory]
    [InlineData("10220")]
    [InlineData("102 20")]
    [InlineData("10-2 250")]

    public void Validator_ForValidPostCode_ShouldNotHaveValidationErrorsForPostCodeProperty(string postCode)
    {
        //arrange
        var validatore = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PotCode = postCode };
        //act
        var result = validatore.TestValidate(command);
        //assert
        result.ShouldHaveValidationErrorFor(c => c.PotCode);
    }
}