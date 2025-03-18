using System.Data;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using ProjectF.Datas;

namespace ProjectF.Networks.DataBases.StoredProcedures
{
    public class WriteUserDataProcedure
    {
        private const string PROCEDURE = "write_user_data";

        private string connectionString = null;
        private DynamicParameters parameters = null;

        public UserData UserData = null;
        public ENetworkResult Result = ENetworkResult.None;
        public string Reason = null;

        public WriteUserDataProcedure(UserData userData)
        {
            connectionString = new CreateGameMySQLConnection().connectionString;
            
            parameters = new DynamicParameters();
            parameters.Add("in_user_id", userData.userID, DbType.String, ParameterDirection.Input);
            parameters.Add("in_user_data", JsonConvert.SerializeObject(userData), DbType.String, ParameterDirection.Input);
        }

        public async Task CallAsync()
        {
            SingleMySQLCommand query = new SingleMySQLCommand(connectionString, PROCEDURE, parameters);
            await query.QueryAsync();

            Result = query.Result;
            Reason = query.Reason;
        }
    }
}