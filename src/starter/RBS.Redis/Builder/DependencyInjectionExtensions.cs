using Rabbit.Boot.Starter.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRedisFactory(this IServiceCollection services)
        {
            return services.AddSingleton<IRedisFactory, RedisFactory>();
        }
    }
}