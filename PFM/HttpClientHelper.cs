using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Polly;
using System.Net;
using Polly.Retry;
using PFM.Models.InputDtos;
using PFM.Models.Dtos;
using System.Security.Claims;

namespace PFM
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientHelper(IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.DefaultRequestHeaders.Clear();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TEntity> Get<TEntity>(string url)
        {
            var policy = CreateTokenRefreshPolicy();

            var result = await policy.ExecuteAsync(async () => 
            {
                SetAuthHeader();
                return await _client.GetAsync(url);
            });

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

        private AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicy()
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(1, async (result, retryCount, context) =>
                {
                    var refreshToken = _httpContextAccessor.HttpContext.Session.GetString("refresh_token");

                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        await RefreshAccessToken(refreshToken);
                    }
                });

            return policy;
        }

        private async Task RefreshAccessToken(string refreshToken)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var uri = "api/auth/refresh-token";
            var refreshData = new RefreshTokenInputDto
            {
                Email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value,
                RefreshToken = refreshToken
            };
            var newTokens = await Post<RefreshTokenInputDto, LoginDto>(uri, refreshData);

            session.SetString("access_token", newTokens.AccessToken.Token);
            session.SetString("refresh_token", newTokens.RefreshToken);
        }


        private string RetrieveAccessTokenFromUser()
        {
            //return _httpContextAccessor.HttpContext.User.FindFirst("access_token")?.Value;

            return _httpContextAccessor.HttpContext.Session.GetString("access_token");
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
