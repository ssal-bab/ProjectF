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

        public async Task<DBRowViewer<TDataType>> GetDataAsync<TDataType>(string tableKey, string rowKey) where TDataType : class, new()
        {
            DBRowViewer<TDataType> rowViewer = new DBRowViewer<TDataType>(CurrentDB, tableKey, rowKey);
            if(rowViewer.Result != ENetworkResult.Success)
                return rowViewer;

            await rowViewer.ReadAsync();
            if(rowViewer.Result == ENetworkResult.DataNotFound)
            {
                // sql에서 가져오기
                // sql에도 없으면 새로 생성한다
                rowViewer.Data = new TDataType();
                await rowViewer.WriteAsync();
            }

            return rowViewer;
        }
    }
}