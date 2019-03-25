using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Database;
using Microsoft.AspNetCore.Identity;
using CrowdLending.Models;
using Microsoft.EntityFrameworkCore;

namespace CrowdLending.Services
{
    public class DefaultInvestmentService : IInvestmentService
    {
        private readonly DefaultDbContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public DefaultInvestmentService(DefaultDbContext context, UserManager<UserEntity> userManager)
        {
            this._context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<InvestmentEntity>> GetInvestmentsByProjectIdAsync(Guid projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
                return Enumerable.Empty<InvestmentEntity>();

            return await _context.Investments
                .Include(i => i.Investor)
                .Where(i => i.Project == project)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<InvestmentEntity>> GetInvestmentsByUserAsync(UserEntity user)
        {
            return await _context.Investments.Where(i => i.Investor == user).ToListAsync();
        }

        public async Task<Guid> CreateInvestmentAsync(
            Guid userId,
            Guid projectId,
            decimal amount)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new InvalidOperationException("You must be logged in.");

            var project = await _context.Projects
                .SingleOrDefaultAsync(p => p.Id == projectId);
            if (project == null) throw new ArgumentException("Invalid project ID.");


            var id = Guid.NewGuid();

            var newInvestment = _context.Investments.Add(new InvestmentEntity()
            {
                Id = id,
                Project = project,
                Investor = user,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = amount
            });

            // Update project collected money
            project.CollectedAmount += amount;

            var created = await _context.SaveChangesAsync();
            if (created < 1) throw new InvalidOperationException("Could not save investment.");

            return id;
        }

        public async Task<(bool, string)> IsProjectInvestmentValid(ProjectEntity project, UserEntity user, decimal amount)
        {
            // Check that user has not invested in the project already
            var userInvestments = await GetInvestmentsByUserAsync(user);
            if (userInvestments.Any(i => i.Project == project))
            {
                return (false, "You have already invested in this project.");
            }

            // Check if Project is already funded
            if (project.CollectedAmount >= project.RequestedAmount)
            {
                return (false, "Project is already fully funded.");
            }

            // Check that amount is not too big
            if (amount > (project.RequestedAmount - project.CollectedAmount))
            {
                return (false, "Amount to invest is greater than remaining request.");
            }

            return (true, null);
        }
    }
}
