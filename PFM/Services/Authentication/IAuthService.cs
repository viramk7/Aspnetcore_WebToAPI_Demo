using System.Threading.Tasks;
using PFM.Models.Dtos;
using PFM.Models.InputDtos;

namespace PFM.Services
{
    public interface IAuthService
    {
        Task<LoginDto> Login(LoginInputDto data);
    }
}