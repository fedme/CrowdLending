using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Models;

namespace CrowdLending.Services
{
    public interface IInvestmentService
    {
        Task<IEnumerable<InvestmentEntity>> GetInvestmentsByProjectIdAsync(Guid projectId);
        Task<IEnumerable<InvestmentEntity>> GetInvestmentsByUserAsync(UserEntity user);
        Task<Guid> CreateInvestmentAsync(Guid userId, Guid projectId, decimal amount);
        Task<(bool, string)> IsProjectInvestmentValid(ProjectEntity project, UserEntity user, decimal amount);
    }
}
