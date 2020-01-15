using PFM.Models.Common;
using System.Threading.Tasks;

namespace PFM.API.Auth
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}