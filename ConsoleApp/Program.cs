using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Boot.Starter.IdentityServer.Client;
using Rabbit.Boot.Starter.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

[assembly: HostingStartup(typeof(RedisHostingStartup))]
[assembly: HostingStartup(typeof(IdentityServerClientHostingStartup))]

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("Redis:ConnectionString", "xxxxxx"),
                        new KeyValuePair<string, string>("Client:Url", "xxxxxx"),
                        new KeyValuePair<string, string>("Client:Identity:Url", "xxxxxx"),
                        new KeyValuePair<string, string>("Client:Identity:ClientId", "xxxxxx"),
                        new KeyValuePair<string, string>("Client:Identity:ClientSecret", "xxxxxx")
                    });
                })
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                var services = app.ApplicationServices;
                var redisFactory = services.GetRequiredService<IRedisFactory>();
                Console.WriteLine(redisFactory.Default());
                Console.WriteLine(services.GetService<IConnectionMultiplexer>());

                var httpClient = services.GetService<IdentityServerHttpClient>();

                Console.WriteLine(httpClient.GetStringAsync("/wechat/accesstoken/wxxxx").GetAwaiter().GetResult());
            }
        }
    }
}