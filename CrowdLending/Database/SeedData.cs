using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrowdLending.Database
{
    public static class SeedData
    {

        public static async Task InitializeAsync(IServiceProvider services)
        {
            await AddMockUsers(
                services.GetRequiredService<RoleManager<UserRoleEntity>>(),
                services.GetRequiredService<UserManager<UserEntity>>());

            await AddMockProjects(
                services.GetRequiredService<DefaultDbContext>(),
                services.GetRequiredService<UserManager<UserEntity>>());
        }

        private static async Task AddMockUsers(
            RoleManager<UserRoleEntity> roleManager,
            UserManager<UserEntity> userManager
        )
        {
            var dataExists = roleManager.Roles.Any() || userManager.Users.Any();
            if (dataExists) return;

            // Add test user roles
            await roleManager.CreateAsync(new UserRoleEntity("ProjectOwner"));
            await roleManager.CreateAsync(new UserRoleEntity("User"));

            // Add a test projectOwner
            var user1 = new UserEntity()
            {
                Email = "john@test.local",
                UserName = "john@test.local",
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user1, "Password123!");
            await userManager.AddToRoleAsync(user1, "ProjectOwner");
            await userManager.UpdateAsync(user1);

            // Add a test normal user
            var user2 = new UserEntity()
            {
                Email = "dave@test.local",
                UserName = "dave@test.local",
                FirstName = "Dave",
                LastName = "White",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user2, "Password123!");
            await userManager.AddToRoleAsync(user2, "User");
            await userManager.UpdateAsync(user2);
        }

        private static async Task AddMockProjects(DefaultDbContext ctx, UserManager<UserEntity> userManager)
        {
            if (ctx.Projects.Any()) return; // db already has data

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "john@test.local");

            // Mock project 1
            var project = new ProjectEntity
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d64"),
                Name = "Help us launch our new product",
                Description = "We will use the money to launch Nucleus, our new smartphone app!",
                RequestedAmount = 15000m,
                CollectedAmount = 7000m,
                Owner = user,
                CreatedAt = DateTimeOffset.UtcNow
            };
            ctx.Projects.Add(project);

            ctx.Investments.Add(new InvestmentEntity()
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d39"),
                Amount = 2000m,
                Investor = user,
                Project = project,
                CreatedAt = DateTimeOffset.UtcNow
            });

            ctx.Investments.Add(new InvestmentEntity()
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d93"),
                Amount = 500m,
                Investor = user,
                Project = project,
                CreatedAt = DateTimeOffset.UtcNow
            });


            // Mock project 2
            ctx.Projects.Add(new ProjectEntity
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d65"),
                Name = "Build a new park in our city",
                Description = "Help us build a new amazing park in our city!",
                RequestedAmount = 200000m,
                CollectedAmount = 15000m,
                Owner = user,
                CreatedAt = DateTimeOffset.UtcNow
            });

            // Mock project 3
            ctx.Projects.Add(new ProjectEntity
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d66"),
                Name = "Found our new Berlin office",
                Description = "We will use the money to open a new branch in the center of Berlin.",
                RequestedAmount = 500000m,
                CollectedAmount = 499000m,
                Owner = user,
                CreatedAt = DateTimeOffset.UtcNow
            });

            await ctx.SaveChangesAsync();
        }
    }
}
