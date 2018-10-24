using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cyclr.LaunchExample.Models
{
    public class Opportunity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public decimal Value { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        public string Status { get; set; }

        [Required]
        public virtual Contact Contact { get; set; }
    }

    public class AddOrUpdateOpportunity
    {
        public Opportunity Opportunity { get; set; }
        public IEnumerable<SelectListItem> Contacts { get; set; }
    }
}