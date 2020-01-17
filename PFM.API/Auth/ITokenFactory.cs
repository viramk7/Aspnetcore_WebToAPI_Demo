namespace PFM.API.Auth
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}