using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.API.Tests;
using Moq;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests :IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock = new();
    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository), _ => _restaurantRepositoryMock.Object));
            });
        });
    }
    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        //arrange
        var id = 11223;

        _restaurantRepositoryMock.Setup(m => m.GetRestaurantByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();

        //act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200OK()
    {
        //arrange
        var id = 99;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test Description",
        };

        _restaurantRepositoryMock.Setup(m => m.GetRestaurantByIdAsync(id)).ReturnsAsync((restaurant));
        var client = _factory.CreateClient();

        //act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto= await response.Content.ReadFromJsonAsync<RestaurantDTO>();   

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test Description");
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Return200Ok()
    {//arrange
        var client= _factory.CreateClient();
        //act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

    }
    [Fact()]
    public async Task GetAll_ForInValidRequest_Return400BadRequest()
    {//arrange
        var client = _factory.CreateClient();
        //act
        var result = await client.GetAsync("/api/restaurants");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

    }
}