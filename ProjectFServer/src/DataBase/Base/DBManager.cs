using RedLockNet;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ProjectF.Networks.DataBases
{
    public class DBManager
    {
        private readonly IRedisClient redisClient = null;
        private readonly IDistributedLockFactory redLockFactory = null;

        public IRedisDatabase CurrentDB => redisClient.Db0;

        private readonly AsyncLock userDataInfoCacheLock = null;
        private Dictionary<string, UserDataInfo> userDataInfoCache = null;

        public DBManager(IRedisClient redisClient, IDistributedLockFactory redLockFactory)
        {
            this.redisClient = redisClient;
            this.redLockFactory = redLockFactory;

            userDataInfoCacheLock = new AsyncLock();
            userDataInfoCache = new Dictionary<string, UserDataInfo>();
        }

        public async Task AddUserDataInfoIntoCache(string userID, UserDataInfo userDataInfo)
        {
            using(await userDataInfoCacheLock.LockAsync())
            {
                if (userDataInfoCache.ContainsKey(userID))
                    userDataInfoCache.Remove(userID);

                userDataInfoCache.Add(userID, userDataInfo);
            }
        }

        public async Task<UserDataInfo> GetUserDataInfo(string userID)
        {
            if(userDataInfoCache.TryGetValue(userID, out UserDataInfo userDataInfo) == false)
            {
                userDataInfo = new UserDataInfo(CurrentDB, userID);
                using (IRedLock redLock = await userDataInfo.LockAsync(redLockFactory))
                {
                    await userDataInfo.ReadAsync();
                    if (userDataInfo.Result != ENetworkResult.Success)
                        return null;

                    await AddUserDataInfoIntoCache(userID, userDataInfo);
                }
            }

            return userDataInfo;
        }
    }
}