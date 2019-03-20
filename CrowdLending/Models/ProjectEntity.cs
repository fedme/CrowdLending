using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdLending.Models
{
    public class ProjectEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserEntity Owner { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal CollectedAmount { get; set; } = 0.00m;
        public decimal InterestRate { get; set; } = 7.4m;
    }
}
