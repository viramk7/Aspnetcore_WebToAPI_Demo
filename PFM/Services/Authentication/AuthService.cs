using PFM.Models.Dtos;
using PFM.Models.InputDtos;
using PFM.Models.OutputDtos;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientHelper _httpClient;

        public AuthService(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginDto> Login(LoginInputDto data)
        {
            var uri = "api/auth/login";
            return await _httpClient.Post<LoginInputDto, LoginDto>(uri, data);
        }
    }
}
