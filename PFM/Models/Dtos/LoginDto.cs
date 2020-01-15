using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Models.Dtos
{
    public class LoginDto
    {
        public LoginDto()
        {
            Errors = new List<Error>();
        }

        public bool Success { get; set; }
        public string Message { get; set; }

        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }

    public class AccessToken
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

}
