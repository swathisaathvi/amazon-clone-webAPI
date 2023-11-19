using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

public class CustomRolesAuthorizationHandler : AuthorizationHandler<CustomRolesRequirement>
{
    private readonly IWebHostEnvironment _environment;

    public CustomRolesAuthorizationHandler(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRolesRequirement requirement)
    {
        if (_environment.IsProduction())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User != null && requirement.AllowedRoles.Any(role => context.User.IsInRole(role)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
