using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;


namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    //TestMethod_senario_expectiaon
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRoleTest_withMatchingRole_shouldReturnTrue(string roleName)
    {
        //arange
        var currentUser = new CurrentUser("1", "test@test", [UserRoles.Admin, UserRoles.User],null,null );

        //act
        var isInRole=currentUser.IsInRole(roleName);

        //assert
        isInRole.Should().BeTrue();

    }
    [Fact()]
    public void IsInRoleTest_withNoMatchingRole_shouldReturnFalse()
    {
        //arange
        var currentUser = new CurrentUser("1", "test@test", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        //assert
        isInRole.Should().BeFalse();

    }
    [Fact()]
    public void IsInRoleTest_withNoMatchingRoleCase_shouldReturnFalse()
    {
        //arange
        var currentUser = new CurrentUser("1", "test@test", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        //assert
        isInRole.Should().BeFalse();

    }
}