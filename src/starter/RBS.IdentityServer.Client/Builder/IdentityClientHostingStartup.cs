using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Boot.Starter.IdentityServer.Client
{
    public class IdentityClientHostingStartup : IHostingStartup
    {
        #region Implementation of IHostingStartup

        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration.GetSection("Rabbit:IdentityClient");
                services
                    .Configure<IdentityClientOptions>(configuration)
                    .AddSingleton<IdentityHttpClientHandler>()
                    .AddSingleton<IdentityHttpClient>();
            });
        }

        #endregion Implementation of IHostingStartup
    }
}