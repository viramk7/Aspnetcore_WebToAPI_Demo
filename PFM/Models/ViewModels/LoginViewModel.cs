using System.ComponentModel.DataAnnotations;

namespace PFM.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}
