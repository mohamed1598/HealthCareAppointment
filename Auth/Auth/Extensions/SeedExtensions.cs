using Auth.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;

namespace Auth.Extensions;

public static class SeedExtensions
{
    public async static Task<WebApplication> Seed(this WebApplication app)
    {
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await DefaultRoles.SeedRolesAsync(roleManager);
        return app;
    }
}
