using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Application.Restaurants.Queries.GetAllRestaturants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;
namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]

public class RestaurantsController(IMediator mediator): ControllerBase
{
    [HttpGet]
    //[ProducesResponseType(StatusCodes.Status200OK,Type =typeof(IEnumerable<RestaurantDTO>))]
    //[Authorize(Policy = PolicyNames.CreatedAtLeast2Restaurants)]
    [AllowAnonymous]
    public  async Task<ActionResult<IEnumerable<RestaurantDTO>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        //get retaurants
        var restaurants = await mediator.Send(query);

        return Ok(restaurants);//retaurants
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.HasNationaHlity)]
    public  async Task<ActionResult<RestaurantDTO?>> GetRestaurantByID(int id)
    {
   
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);//
    }

    [HttpPost]
    [Authorize(Roles =UserRoles.Owner)]
    //[Authorize(Policy = PolicyNames.CreatedAtLeast2Restaurants)]
    public  async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetRestaurantByID), new { id }, null);
            

    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute]int id,UpadteRestaurantCommand command)
    {
      command.Id = id;
      await mediator.Send(command);
      return NoContent(); 

      
    }

}
