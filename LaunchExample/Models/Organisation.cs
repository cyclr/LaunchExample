using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyclr.LaunchExample.Models
{
    public class Organisation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}