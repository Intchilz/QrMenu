using Microsoft.AspNetCore.Identity;
using QrMenu.Models;

namespace QrMenu.Services;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles =
        {
            ApplicationRoles.SuperAdmin,
            ApplicationRoles.Admin,
            ApplicationRoles.Manager,
            ApplicationRoles.Staff
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole<Guid>(role));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create role '{role}': " +
                        string.Join(
                            ", ",
                            result.Errors.Select(
                                error => error.Description)));
                }
            }
        }

        var email =
            configuration["QRMENU_SUPERADMIN_EMAIL"];

        var password =
            configuration["QRMENU_SUPERADMIN_PASSWORD"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "QRMENU_SUPERADMIN_EMAIL and " +
                "QRMENU_SUPERADMIN_PASSWORD " +
                "must be configured.");
        }

        var existingSuperAdmin =
            await userManager.FindByEmailAsync(email);

        if (existingSuperAdmin == null)
        {
            var superAdmin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "QrMenu",
                LastName = "SuperAdmin",
                RestaurantId = null,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var createResult =
                await userManager.CreateAsync(
                    superAdmin,
                    password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to create the SuperAdmin account: " +
                    string.Join(
                        ", ",
                        createResult.Errors.Select(
                            error => error.Description)));
            }

            var roleResult =
                await userManager.AddToRoleAsync(
                    superAdmin,
                    ApplicationRoles.SuperAdmin);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to assign the SuperAdmin role: " +
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(
                            error => error.Description)));
            }
        }
        else if (!await userManager.IsInRoleAsync(
                     existingSuperAdmin,
                     ApplicationRoles.SuperAdmin))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    existingSuperAdmin,
                    ApplicationRoles.SuperAdmin);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to assign the SuperAdmin role: " +
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(
                            error => error.Description)));
            }
        }
    }
}