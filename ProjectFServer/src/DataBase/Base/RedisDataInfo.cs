using System;
using System.Threading.Tasks;
using RedLockNet;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ProjectF.Networks.DataBases
{
    public abstract class RedisDataInfo<TDataType> where TDataType : class
    {
        protected IRedisDatabase db = null;
        protected string tableKey = null;
        protected string rowKey = null;

        public TDataType Data = null;
        public ENetworkResult Result = ENetworkResult.None;
        public string Reason = null;

        public RedisDataInfo(IRedisDatabase db, string tableKey, string rowKey)
        {
            if(string.IsNullOrEmpty(tableKey))
            {
                Result = ENetworkResult.DBError;
                Reason = "Given table key is null or empty";
                return;
            }

            if(string.IsNullOrEmpty(rowKey))
            {
                Result = ENetworkResult.DBError;
                Reason = "Given row key is null or empty";
                return;
            }

            this.db = db;
            this.tableKey = tableKey;
            this.rowKey = rowKey;

            Result = ENetworkResult.Success;
        }

        public async Task<IRedLock> LockAsync(IDistributedLockFactory redLockFactory)
        {
            string resourceName = $"{tableKey}_{rowKey}";
            TimeSpan expiryTime = TimeSpan.FromSeconds(30);
            TimeSpan waitTime = TimeSpan.FromSeconds(10);
            TimeSpan retryTime = TimeSpan.FromSeconds(1);
            return await redLockFactory.CreateLockAsync(resourceName, expiryTime, waitTime, retryTime);
        }

        public async Task ReadAsync()
        {
            try {
                Data = await db.HashGetAsync<TDataType>(tableKey, rowKey);
                bool success = Data != null;
                Result = success ? ENetworkResult.Success : ENetworkResult.DataNotFound;
            }
            catch(RedisTimeoutException _)
            {
                Result = ENetworkResult.Timeout;
                Reason = "Timeout";
            }
            catch(Exception err)
            {
                Result = ENetworkResult.DBError;
                Reason = err.Message;
            }
        }

        public async Task WriteAsync(bool twoWayWrite = true)
        {
            try {
                if(twoWayWrite)
                    SideWayWrite();

                await db.HashSetAsync(tableKey, rowKey, Data);
                Result = ENetworkResult.Success;
            }
            catch(RedisTimeoutException _)
            {
                Result = ENetworkResult.Timeout;
                Reason = "Timeout";
            }
            catch(Exception err)
            {
                Result = ENetworkResult.Error;
                Reason = err.Message;
            }
        }

        protected abstract void SideWayWrite();
    }
}