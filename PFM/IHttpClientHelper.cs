using System.Threading.Tasks;

namespace PFM
{
    public interface IHttpClientHelper
    {
        Task<TEntity> Get<TEntity>(string url);

        Task Post<Tin>(string uri, Tin entity);

        Task<TOut> Post<Tin, TOut>(string uri, Tin entity);
    }
}