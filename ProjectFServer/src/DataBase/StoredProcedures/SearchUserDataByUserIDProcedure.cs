using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using ProjectF.Datas;

namespace ProjectF.Networks.DataBases.StoredProcedures
{
    public class SearchUserDataByUserIDProcedure
    {
        private class QueryResult
        {
            public string user_data;
        }

        private const string PROCEDURE = "search_user_data_by_user_id";

        private string connectionString = null;
        private DynamicParameters parameters = null;

        public UserData UserData = null;
        public ENetworkResult Result = ENetworkResult.None;
        public string Reason = null;

        public SearchUserDataByUserIDProcedure(string userID)
        {
            connectionString = new CreateGameMySQLConnection().connectionString;
            
            parameters = new DynamicParameters();
            parameters.Add("in_user_id", userID, DbType.String, ParameterDirection.Input, 36);
        }

        public async Task CallAsync()
        {
            SingleMySQLQuery query = new SingleMySQLQuery(connectionString, PROCEDURE, parameters);
            List<QueryResult> resultList = await query.QueryAsync<QueryResult>();
            
            if(query.Result != ENetworkResult.Success)
            {
                Result = query.Result;
                Reason = query.Reason;
                return;
            }

            if(resultList.Count <= 0)
            {
                Result = ENetworkResult.DataNotFound;
                Reason = "Data not found";
                return;
            }

            if(resultList.Count > 1)
            {
                Result = ENetworkResult.DBError;
                Reason = "Multiple data found. Wrong query.";
                return;
            }

            UserData = JsonConvert.DeserializeObject<UserData>(resultList[0].user_data);
            Result = UserData == null ? ENetworkResult.DBError : ENetworkResult.Success;
        }
    }
}