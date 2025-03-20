using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entities;
using DataAccessLayer.Data;

namespace DataAccessLayer.Seed
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = new User
            {
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            if (!await context.AboutContents.AnyAsync())
            { 
                context.AboutContents.Add(new AboutContent
                {
                    Description = "This URL shortener generates a random string of 7 characters (A-Z, a-z, 0-9) for each URL. " +
                            "It ensures uniqueness by checking the database for existing codes before assigning one."
                });
                await context.SaveChangesAsync();
            }
        }
    }

}
