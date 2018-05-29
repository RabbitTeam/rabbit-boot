using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Boot.Starter.IdentityServer.Client.Internal
{
    internal abstract class AccessTokenAccessor
    {
        private TokenEntry _tokenEntry;

        public async Task<string> GetTokenAsync(bool ignoreCache = false)
        {
            if (!_tokenEntry.IsExpired && ignoreCache == false)
                return _tokenEntry.Token;

            var response = await RequestAccessTokenAsync();

            if (string.IsNullOrEmpty(response))
                throw new Exception("无法获取AccessToken。");

            var obj = JObject.Parse(response);

            _tokenEntry = new TokenEntry(obj.Value<string>("access_token"), obj.Value<int>("expires_in"));
            return _tokenEntry.Token;
        }

        protected abstract Task<string> RequestAccessTokenAsync();

        #region Help Type

        private struct TokenEntry
        {
            private readonly long _expiration;

            public TokenEntry(string token, int expiresIn)
            {
                Token = token;
                _expiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + expiresIn;
            }

            public string Token { get; }
            internal bool IsExpired => DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= _expiration - 10;
        }

        #endregion Help Type
    }

    internal class IdentityAccessTokenAccessor : AccessTokenAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityClientOptions _options;

        public IdentityAccessTokenAccessor(IdentityClientOptions options) : this(options, new HttpClient())
        {
        }

        public IdentityAccessTokenAccessor(IdentityClientOptions options, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = options;
        }

        #region Overrides of AccessTokenAccessor

        /// <inheritdoc/>
        protected override async Task<string> RequestAccessTokenAsync()
        {
            var baseUrl = _options.IdentityUrl;
            var clientId = _options.ClientId;
            var clientSecret = _options.ClientSecret;

            var url = baseUrl + "/connect/token";

            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials"}
                })
            };

            message.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret)));

            var response = await _httpClient.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }

        #endregion Overrides of AccessTokenAccessor
    }
}