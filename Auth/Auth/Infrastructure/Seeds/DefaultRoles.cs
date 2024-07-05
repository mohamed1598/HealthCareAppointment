using Auth.Constants;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Seeds;

public static class DefaultRoles
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(AppRoles.NoProfileUser));
        }
    }
}
