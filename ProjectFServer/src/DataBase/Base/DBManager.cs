using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ProjectF.Networks.DataBases
{
    public class DBManager
    {
        private readonly IRedisClient redisClient = null;

        public IRedisDatabase CurrentDB => redisClient.Db0;

        public DBManager(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }
    }
}