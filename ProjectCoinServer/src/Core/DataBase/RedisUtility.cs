using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace ProjectCoin.Networks.DataBases
{
    public struct CreateRedisConfiguration
    {
        public RedisConfiguration configuration;

        public CreateRedisConfiguration()
        {
            configuration = new RedisConfiguration()
            {
                AbortOnConnectFail = false,
                // 우선은 하드코딩 하지만 나중에 바꿔야 한다.
                Hosts = new RedisHost[] {
                    new RedisHost() { Host = "SEH00N.iptime.org", Port = 8530 }
               },
                AllowAdmin = true,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                Database = 0,
                Ssl = false,
                // 이것도 마찬가지
                Password = "redis30928530@",
                PoolSize = 50,
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                }
            };
        }
    }

    public struct CreateRedLockFactory
    {
        public IDistributedLockFactory Create(IServiceProvider provider)
        {
            List<RedLockMultiplexer> connectionList = new()
            {
                new RedLockMultiplexer(provider.GetRequiredService<IRedisConnectionPoolManager>().GetConnection())
            };
            RedLockFactory redLockFactory = RedLockFactory.Create(connectionList);
            return redLockFactory;
        }
    }
}