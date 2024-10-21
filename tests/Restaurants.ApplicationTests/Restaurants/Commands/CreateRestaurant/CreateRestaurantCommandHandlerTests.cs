using Xunit;

using Moq;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Restaurants.Application.Users;
using FluentAssertions;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task  Handle_ForValidCommand_ReturnsCreateRestaurantId()
    {
        //araange

        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var command = new CreateRestaurantCommand() ;
        var restaurant = new Restaurant();

        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        var restarantRepositoryMock = new Mock<IRestaurantRepository>();
        restarantRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<Restaurant>()))
            .ReturnsAsync(1);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@test.com", [], null, null);
        userContextMock.Setup(u=>u.GetCurrentUser()).Returns(currentUser);


        var commadndHandler = new CreateRestaurantCommandHandler(
            restarantRepositoryMock.Object,
            loggerMock.Object,
            mapperMock.Object,
            userContextMock.Object);
      
        //act
        var result = await commadndHandler.Handle(command, CancellationToken.None);

        //assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restarantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);
            
            
    } 
}