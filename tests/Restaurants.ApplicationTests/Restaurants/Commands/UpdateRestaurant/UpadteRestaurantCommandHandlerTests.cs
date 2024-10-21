using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;


namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpadteRestaurantCommandHandlerTests
{


    private readonly Mock<ILogger<UpadteRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
    private readonly UpadteRestaurantCommandHandler _handler;
    public UpadteRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpadteRestaurantCommandHandler>>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();

        _handler = new UpadteRestaurantCommandHandler(
            _restaurantRepositoryMock.Object
            , _loggerMock.Object
            , _mapperMock.Object
            , _restaurantAuthorizationServiceMock.Object);
    }

    [Fact()]
    public async void Handle_withValidRequest_ShouldUpdateRestaurants()
    {

        //arrange
        var restaurantId = 1;
        var command = new UpadteRestaurantCommand()
        {
            Id = restaurantId,
            Name = "New Test",
            Description = "New Descrption",
            HasDelivery = true
        };
        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test"
        };
        _restaurantRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Updete))
            .Returns(true);

        //act
        await _handler.Handle(command, CancellationToken.None);

        //assert
        _restaurantRepositoryMock.Verify(r => r.saveChanges(), Times.Once());
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once());

    }

  
    [Fact()]
    public async void Handle_withUnAuthoriedUser_ShouldThrowNotFoundException()
    {
        //arange
        var restaurantId = 3;
        var request = new UpadteRestaurantCommand
        {
            Id= restaurantId
        };
        var existingRestaurant = new Restaurant
        {
            Id = restaurantId
        };
        _restaurantRepositoryMock.Setup(r => r.GetRestaurantByIdAsync(restaurantId))
            .ReturnsAsync(existingRestaurant);
        _restaurantAuthorizationServiceMock
            .Setup(a => a.Authorize(existingRestaurant, ResourceOperation.Updete))
            .Returns(false);
        //act
        Func<Task> act= async () => await _handler.Handle(request, CancellationToken.None);
        //assert
        await act.Should().ThrowAsync<ForbidException>();

    }  
}