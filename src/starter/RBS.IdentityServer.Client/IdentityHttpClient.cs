using Microsoft.Extensions.Options;
using Rabbit.Boot.Starter.IdentityServer.Client.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit.Boot.Starter.IdentityServer.Client
{
    public class IdentityHttpClientHandler : HttpClientHandler
    {
        private readonly IdentityAccessTokenAccessor _accessTokenAccessor;

        public IdentityHttpClientHandler(IOptions<IdentityClientOptions> optionsAccessor)
        {
            _accessTokenAccessor = new IdentityAccessTokenAccessor(optionsAccessor.Value);
        }

        #region Overrides of HttpMessageHandler

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenAccessor.GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return await base.SendAsync(request, cancellationToken);
        }

        #endregion Overrides of HttpMessageHandler
    }

    public class IdentityHttpClient : HttpClient
    {
        public IdentityHttpClient(IOptions<IdentityClientOptions> optionsAccessor, IdentityHttpClientHandler httpClientHandler) : base(httpClientHandler)
        {
            var baseUrl = optionsAccessor.Value.BaseUrl;
            if (!string.IsNullOrWhiteSpace(baseUrl))
                BaseAddress = new Uri(baseUrl);
        }
    }
}