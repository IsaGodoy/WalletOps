using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalletOps.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles = ["SystemAdmin", "BankOfficer", "Customer"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            const string adminEmail = "admin@walletops.local";
            const string adminPassword = "Admin123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
                }
            }
            const string officerEmail = "officer@walletops.local";
            const string officerPassword = "Officer123!";
            var officerUser = await userManager.FindByEmailAsync(officerEmail);
            if (officerUser is null)
            {
                officerUser = new ApplicationUser
                {
                    UserName = officerEmail,
                    Email = officerEmail,
                    FullName = "Bank Officer",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(officerUser, officerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(officerUser, "BankOfficer");
                }
            }
        }
    }
}
