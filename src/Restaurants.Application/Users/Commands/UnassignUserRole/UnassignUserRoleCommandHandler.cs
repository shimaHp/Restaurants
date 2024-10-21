

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.DeleteUserRole;

public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger,

IMapper mapper,
      UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user=await userManager.FindByEmailAsync(request.UserEmail) 
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        var role= await roleManager.FindByNameAsync(request.RoleName)
              ?? throw new NotFoundException(nameof(roleManager), request.RoleName);

       await userManager.RemoveFromRoleAsync(user!, role.Name!);

    }
}
