using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PFM.Models.InputDtos
{
    public class RefreshTokenInputDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
