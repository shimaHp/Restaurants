

using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.DTOs;

public class RestaurantsProfile: Profile
{
    public RestaurantsProfile()
    {
        CreateMap<UpadteRestaurantCommand, Restaurant>();

        CreateMap<CreateRestaurantCommand,Restaurant>()
            .ForMember(d=>d.Address,opt=>opt.MapFrom(
                src=>new Address 
                { 
                City=src.City,
                PotCode=src.PotCode,
                Street=src.Street,
                }));




        CreateMap<Restaurant,RestaurantDTO>()
            .ForMember(d=>d.City,opt=>
               opt.MapFrom(src=> src.Address == null ?  null :src.Address.City))
            .ForMember(d => d.City, opt =>
               opt.MapFrom(src => src.Address == null ? null : src.Address.PotCode))
            .ForMember(d => d.City , opt =>
               opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.Dishes, opt =>
               opt.MapFrom(src => src.Dishes == null ? null : src.Dishes))
            ;
    }
}
