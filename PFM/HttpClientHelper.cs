using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace PFM
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientHelper(IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44355/");
            _client.DefaultRequestHeaders.Clear();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TEntity> Get<TEntity>(string url)
        {
            SetAuthHeader();
            var result = await _client.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<TEntity>();
            }

            return default;
        }

        public async Task Post<Tin>(string uri, Tin entity)
        {
            SetAuthHeader();
            var result = await _client.PostAsJsonAsync(uri, entity);
            result.EnsureSuccessStatusCode();
        }

        public async Task<Tout> Post<TIn, Tout>(string uri, TIn entity)
        {
            SetAuthHeader();
            var result = await _client.PostAsJsonAsync(uri, entity);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<Tout>();
            }

            return default;
        }

        #region Private Methods
        
        private string RetrieveAccessTokenFromUser()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("access_token")?.Value;
        }

        private void SetAuthHeader()
        {
            var accessToken = RetrieveAccessTokenFromUser();
            if (!string.IsNullOrEmpty(RetrieveAccessTokenFromUser()))
            {
                _client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        #endregion

    }
}
