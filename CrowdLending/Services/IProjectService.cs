using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Models;

namespace CrowdLending.Services
{
    public interface IProjectService
    {
        Task<ProjectEntity> GetProjectAsync(Guid id);
        Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync();
        Task<IEnumerable<ProjectEntity>> GetAllProjectsWithOwnersAsync();
    }
}
