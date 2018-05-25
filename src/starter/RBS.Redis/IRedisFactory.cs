using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Rabbit.Boot.Starter.Redis
{
    public interface IRedisFactory
    {
        IConnectionMultiplexer Get(string name);
    }

    public static class RedisFactoryExtensions
    {
        public static IConnectionMultiplexer Default(this IRedisFactory redisFactory)
        {
            return redisFactory.Get(Options.DefaultName);
        }
    }
}