using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace Rabbit.Boot.Starter.Redis
{
    public class RedisFactory : IRedisFactory, IDisposable
    {
        private readonly IOptionsMonitor<RedisOptions> _optionsMonitor;
        private readonly ConcurrentDictionary<string, Lazy<IConnectionMultiplexer>> _connectionMultiplexers = new ConcurrentDictionary<string, Lazy<IConnectionMultiplexer>>();

        public RedisFactory(IOptionsMonitor<RedisOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
            optionsMonitor.OnChange((_, name) => _connectionMultiplexers.TryRemove(name, out var _));
        }

        #region Implementation of IRedisFactory

        /// <inheritdoc/>
        public IConnectionMultiplexer Get(string name)
        {
            var options = _optionsMonitor.Get(name);
            if (options == null)
                throw new Exception($"找不到名称为 {name} 的redis选项");

            var item = _connectionMultiplexers.GetOrAdd(name,
                k => new Lazy<IConnectionMultiplexer>(() => Create(options)));
            return item.Value;
        }

        #endregion Implementation of IRedisFactory

        #region Private Method

        private static IConnectionMultiplexer Create(RedisOptions options)
        {
            return ConnectionMultiplexer.Connect(options.ConnectionString);
        }

        #endregion Private Method

        #region IDisposable

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var lazy in _connectionMultiplexers.Values)
            {
                if (!lazy.IsValueCreated)
                    continue;

                try
                {
                    lazy.Value?.Dispose();
                }
                catch
                {
                    // ignored
                }
            }

            _connectionMultiplexers.Clear();
        }

        #endregion IDisposable
    }
}