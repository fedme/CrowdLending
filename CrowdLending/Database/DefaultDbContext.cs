using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrowdLending.Database
{
    public class DefaultDbContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public DefaultDbContext(DbContextOptions options)
            : base(options) {}

        public DbSet<ProjectEntity> Projects { get; set; }

        public DbSet<InvestmentEntity> Investments { get; set; }
    }
}
