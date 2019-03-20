using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Database;
using CrowdLending.Models;
using Microsoft.EntityFrameworkCore;

namespace CrowdLending.Services
{
    public class DefaultProjectService : IProjectService
    {
        private readonly DefaultDbContext _context;

        public DefaultProjectService(DefaultDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToArrayAsync();
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllProjectsWithOwnersAsync()
        {
            return await _context.Projects.Include(p => p.Owner).ToArrayAsync();
        }

        public async Task<ProjectEntity> GetProjectAsync(Guid id)
        {
            return await _context.Projects.
                Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
