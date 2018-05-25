using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Boot.Starter.IdentityServer.Client
{
    public class IdentityServerClientHostingStartup : IHostingStartup
    {
        #region Implementation of IHostingStartup

        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration.GetSection("Client");
                services
                    .Configure<ClientOptions>(configuration)
                    .AddSingleton<IdentityServerHttpClientHandler>()
                    .AddSingleton<IdentityServerHttpClient>();
            });
        }

        #endregion Implementation of IHostingStartup
    }
}