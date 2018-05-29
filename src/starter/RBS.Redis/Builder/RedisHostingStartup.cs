using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Boot.Starter.Redis
{
    public class RedisHostingStartup : IHostingStartup
    {
        #region Implementation of IHostingStartup

        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var redisConfiguration = context.Configuration.GetSection("Rabbit:Redis");
                services
                    .Configure<RedisOptions>(redisConfiguration)
                    .AddRedisFactory()
                    .AddSingleton(s => s.GetRequiredService<IRedisFactory>().Default());
            });
        }

        #endregion Implementation of IHostingStartup
    }
}