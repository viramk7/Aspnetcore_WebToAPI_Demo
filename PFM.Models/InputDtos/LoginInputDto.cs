using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PFM.Models.InputDtos
{
    public class LoginInputDto
    {
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}
