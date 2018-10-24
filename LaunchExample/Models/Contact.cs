using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cyclr.LaunchExample.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        
        public virtual Organisation Organisation { get; set; }
    }

    public class AddOrUpdateContactModel
    {
        public Contact Contact { get; set; }
        public IEnumerable<SelectListItem> Organisations { get; set; }
    }
}