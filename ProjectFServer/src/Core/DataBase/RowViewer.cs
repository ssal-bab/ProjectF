using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ProjectF.Networks.DataBases
{
    public class DBRowViewer<TDataType> where TDataType : class
    {
        protected IRedisDatabase db = null;
        protected string tableKey = null;
        protected string rowKey = null;

        public TDataType Data = null;
        public ENetworkResult Result = ENetworkResult.None;
        public string Reason = null;

        public DBRowViewer(IRedisDatabase db, string tableKey, string rowKey)
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

        public async Task WriteAsync()
        {
            try {
                string serializedData = JsonConvert.SerializeObject(Data);
                
                // sql 저장은 background로 돌린다.
                new WriteHashDataIntoSQL(rowKey, serializedData).ProcessAsync();
                bool success = await db.HashSetAsync(tableKey, rowKey, serializedData);
                Result = success ? ENetworkResult.Success : ENetworkResult.Fail;
            }
            catch(RedisTimeoutException _)
            {
                Result = ENetworkResult.Timeout;
                Reason = "Timeout";
            }
            catch(Exception err)
            {
                Result = ENetworkResult.Fail;
                Reason = err.Message;
            }
        }
    }
}