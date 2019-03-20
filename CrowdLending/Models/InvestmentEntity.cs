using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdLending.Models
{
    public class InvestmentEntity
    {
        public Guid Id { get; set; }
        public ProjectEntity Project { get; set; }
        public UserEntity Investor { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
