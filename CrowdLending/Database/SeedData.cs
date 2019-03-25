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

            // Test project owner 1
            var user1 = new UserEntity()
            {
                Email = "lisa@test.local",
                UserName = "lisa@test.local",
                FirstName = "Lisa",
                LastName = "Moore",
                AvatarSrc = "https://randomuser.me/api/portraits/women/68.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user1, "Password123!");
            await userManager.AddToRoleAsync(user1, "ProjectOwner");
            await userManager.UpdateAsync(user1);

            // Test Project Owner 2
            var user2 = new UserEntity()
            {
                Email = "john@test.local",
                UserName = "john@test.local",
                FirstName = "John",
                LastName = "Doe",
                AvatarSrc = "https://randomuser.me/api/portraits/men/46.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user2, "Password123!");
            await userManager.AddToRoleAsync(user2, "ProjectOwner");
            await userManager.UpdateAsync(user2);

            // Test normal user 1
            var user3 = new UserEntity()
            {
                Email = "dave@test.local",
                UserName = "dave@test.local",
                FirstName = "Dave",
                LastName = "White",
                AvatarSrc = "https://pbs.twimg.com/profile_images/974736784906248192/gPZwCbdS.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user3, "Password123!");
            await userManager.AddToRoleAsync(user3, "User");
            await userManager.UpdateAsync(user3);

            // Test normal user 2
            var user4 = new UserEntity()
            {
                Email = "sarah@test.local",
                UserName = "sarah@test.local",
                FirstName = "Sarah",
                LastName = "Jones",
                AvatarSrc = "https://randomuser.me/api/portraits/women/8.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user4, "Password123!");
            await userManager.AddToRoleAsync(user4, "User");
            await userManager.UpdateAsync(user4);

            // Test normal user 3
            var user5 = new UserEntity()
            {
                Email = "jack@test.local",
                UserName = "jack@test.local",
                FirstName = "Jack",
                LastName = "Green",
                AvatarSrc = "https://tinyfac.es/data/avatars/7D3FA6C0-83C8-4834-B432-6C65ED4FD4C3-500w.jpeg",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user5, "Password123!");
            await userManager.AddToRoleAsync(user5, "User");
            await userManager.UpdateAsync(user5);
        }

        private static async Task AddMockProjects(DefaultDbContext ctx, UserManager<UserEntity> userManager)
        {
            if (ctx.Projects.Any()) return; // db already has data

            // project owners
            var lisa = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "lisa@test.local");
            var john = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "john@test.local");

            // normal users
            var sarah = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "sarah@test.local");
            var jack = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == "jack@test.local");

            // Mock project 1
            var project = new ProjectEntity
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d64"),
                Name = "Help us launch our new app!",
                Description = "We will use the money to launch Nucleus, our new smartphone app!",
                CoverImageSrc = "https://3c1703fe8d.site.internapcdn.net/newman/gfx/news/hires/2017/app.jpg",
                RequestedAmount = 15000m,
                CollectedAmount = 7000m,
                Owner = lisa,
                CreatedAt = DateTimeOffset.UtcNow
            };
            ctx.Projects.Add(project);

            ctx.Investments.Add(new InvestmentEntity()
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d39"),
                Amount = 600m,
                Investor = sarah,
                Project = project,
                CreatedAt = DateTimeOffset.UtcNow
            });

            ctx.Investments.Add(new InvestmentEntity()
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d93"),
                Amount = 500m,
                Investor = jack,
                Project = project,
                CreatedAt = DateTimeOffset.UtcNow
            });


            // Mock project 2
            var project2 = new ProjectEntity
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d65"),
                Name = "Build a new park in our city",
                Description = "Help us build a new amazing park in our city!",
                CoverImageSrc = "https://res.cloudinary.com/simpleview/image/upload/crm/kelowna/Kinsmen-Park-Benches-40ad334c5056a36_40ad3bce-5056-a36a-0b358f00dbd24259.jpg",
                RequestedAmount = 200000m,
                CollectedAmount = 15000m,
                Owner = john,
                CreatedAt = DateTimeOffset.UtcNow
            };
            ctx.Projects.Add(project2);

            ctx.Investments.Add(new InvestmentEntity()
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d84"),
                Amount = 1500m,
                Investor = jack,
                Project = project2,
                CreatedAt = DateTimeOffset.UtcNow
            });

            // Mock project 3
            ctx.Projects.Add(new ProjectEntity
            {
                Id = Guid.Parse("2645611c-fc12-4153-a48a-fdc131673d66"),
                Name = "Found our new Berlin office",
                Description = "We will use the money to open a new branch in the center of Berlin.",
                CoverImageSrc = "https://www.riotgames.com/darkroom/1440/c2f185cb48161e1bb6c6fea45257bdcb:dfdab8b9364f007f0fd96be0b1cf52be/berlin.jpg",
                RequestedAmount = 500000m,
                CollectedAmount = 499000m,
                Owner = lisa,
                CreatedAt = DateTimeOffset.UtcNow
            });

            await ctx.SaveChangesAsync();
        }
    }
}
