using PFM.Models.Common;
using System.Collections.Generic;

namespace PFM.Models.OutputDtos
{
    public class LoginOutputDto : UseCaseResponseMessage
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }
        public IEnumerable<Error> Errors { get; }

        public LoginOutputDto(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public LoginOutputDto(AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
