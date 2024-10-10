

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants.Application.Users.Commands.DeleteUserRole;

public class UnassignUserRoleCommand:IRequest
{
    public string UserEmail { get; set; } = default!;
    public  string RoleName { get; set; } = default!;
}
