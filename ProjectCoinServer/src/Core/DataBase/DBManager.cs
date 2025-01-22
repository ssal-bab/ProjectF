using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ProjectCoin.Networks.DataBases
{
    public class DBManager
    {
        private readonly IRedisClient redisClient = null;

        public DBManager(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        public IRedisDatabase GetCurrentDatabase()
        {
            return redisClient.Db0;
        }
    }
}