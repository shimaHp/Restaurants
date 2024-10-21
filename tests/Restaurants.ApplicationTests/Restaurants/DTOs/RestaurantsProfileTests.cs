using Xunit;
using Restaurants.Application.Restaurants.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Restaurants.Domain.Entities;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

namespace Restaurants.Application.Restaurants.DTOs.Tests;

public class RestaurantsProfileTests
{
    private readonly IMapper _mapper;

    public RestaurantsProfileTests()
    {
        //arrange
        var configration = new MapperConfiguration(cfg => {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantsDTO_MapsCorrectly()
    {
        //arrange
        
        var restaurant = new Restaurant()
        {
            Id= 1,
            Name="Test Restaurant",
            Category="Test categoery",
            Description="test description",
            HasDelivery=true,
            ContactEmail="test@mail.com",
            ContactNumber="123456789",

            Address = new Address
            {
                City = "test",
                Street = "test",
                PotCode = "B86 1GY",
            }

        };
        //act

        var restaurantDto = _mapper.Map<RestaurantDTO>(restaurant);

        //assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
              restaurantDto.Description.Should().Be(restaurant.Description);
        //restaurantDto.Category.Should().Be(restaurant.Category);
        //restaurantDto.City.Should().Be(restaurant.Address.City);
        //restaurantDto.PotCode.Should().Be(restaurant.Address.PotCode);
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
       
        var command = new CreateRestaurantCommand()
        {
       
            Name = "Test Restaurant",
            Category = "Test categoery",
            Description = "test description",
            HasDelivery = true,
            ContactEmail = "test@mail.com",
            ContactNumber = "123456789",
            City = "test",
            Street = "test",
            PotCode = "B86 1GY",


        };
        //act

        var restaurant = _mapper.Map<Restaurant>(command);

        //assert
        restaurant.Should().NotBeNull();
    
        restaurant.Name.Should().Be(command.Name);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.PotCode.Should().Be(command.PotCode);
    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurants_MapsCorrectly()
    {
        //arrange

        var command = new UpadteRestaurantCommand
        {
            Id = 1,
            Name = "updtae  Restaurant",
            Description = "update description",
            HasDelivery = false,
         
        };
        //act

        var restauran = _mapper.Map<Restaurant>(command);

        //assert
        restauran.Should().NotBeNull();
        restauran.Id.Should().Be(command.Id);
        restauran.Name.Should().Be(command.Name);
        restauran.HasDelivery.Should().Be(command.HasDelivery);
        restauran.Description.Should().Be(command.Description);
     
    }
}