using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdLending.Models
{
    public class InvestmentForm
    {
        [Required]
        [Range(100, 10000)]
        [Display(Name = "amount", Description = "Amount to invest")]
        public decimal Amount { get; set; }
    }
}
