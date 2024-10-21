﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Queries.GetAllForRestaurant;

using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurants;
using Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Infrastructure.Authorization;


namespace Restaurants.API.Controllers;

[Route("api/restaurant/{restaurantId}/dishes")]
[ApiController]
[Authorize]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
      var dishId=  await mediator.Send(command);
        return CreatedAtAction(nameof(GetByIdForRestaurant),new { restaurantId, dishId },null);
    }
    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeast20)]
    public async Task<ActionResult<IEnumerable<DishDTO>>> GetAllForRestaurants([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDTO>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dishe = await mediator.Send(new GetDishByIdForRestaurantsQuery(restaurantId, dishId));
        return Ok(dishe);
         
    }

    [HttpDelete]
    public async Task<ActionResult<IEnumerable<DishDTO>>> DeleteAllForRestaurant([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteAllDishesForRestaurantQuery(restaurantId));
        return NoContent();
    }
}