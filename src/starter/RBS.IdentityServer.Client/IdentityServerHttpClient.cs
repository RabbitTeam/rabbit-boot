using Microsoft.Extensions.Options;
using Rabbit.Boot.Starter.IdentityServer.Client.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit.Boot.Starter.IdentityServer.Client
{
    public class IdentityServerHttpClientHandler : HttpClientHandler
    {
        private readonly IdentityAccessTokenAccessor _accessTokenAccessor;

        public IdentityServerHttpClientHandler(IOptions<ClientOptions> optionsAccessor)
        {
            _accessTokenAccessor = new IdentityAccessTokenAccessor(optionsAccessor.Value.Identity);
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

    public class IdentityServerHttpClient : HttpClient
    {
        public IdentityServerHttpClient(IOptions<ClientOptions> optionsAccessor, IdentityServerHttpClientHandler httpClientHandler) : base(httpClientHandler)
        {
            BaseAddress = new Uri(optionsAccessor.Value.Url);
        }
    }
}