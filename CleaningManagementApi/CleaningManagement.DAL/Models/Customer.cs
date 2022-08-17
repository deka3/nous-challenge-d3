using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleaningManagement.DAL.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<CleaningPlan> CleaningPlans { get; set; }
    }
}
