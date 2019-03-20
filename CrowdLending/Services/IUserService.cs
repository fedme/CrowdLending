using CrowdLending.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CrowdLending.Services
{
    public interface IUserService
    {
        Task<Guid?> GetUserIdAsync(ClaimsPrincipal principal);
        Task<UserEntity> GetUserByIdAsync(Guid userId);
        Task<UserEntity> GetUserAsync(ClaimsPrincipal user);
    }
}
