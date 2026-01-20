using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // Storing plain/simple hash for this project as per request
    }
}
