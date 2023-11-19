using Microsoft.AspNetCore.Authorization;

public class CustomRolesRequirement : IAuthorizationRequirement
{
    public string[] AllowedRoles { get; }

    public CustomRolesRequirement(string[] allowedRoles)
    {
        AllowedRoles = allowedRoles;
    }
}
