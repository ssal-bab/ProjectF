using System.Data;
using Dapper;
using Newtonsoft.Json;
using ProjectF.Datas;

namespace ProjectF.Networks.DataBases
{
    public struct GetGameSQLDBConnection
    {
        public string connectionString = null;

        public GetGameSQLDBConnection(string address, int port, string userID, string password, string database)
        {
            connectionString = $"server={address};port={port};uid={userID};password={password};database={database}";
        }
    }

    public struct GetDefaultSQLConnection
    {
        public string connectionString = null;

        public GetDefaultSQLConnection(string database)
        {
            connectionString = new GetGameSQLDBConnection(
                "SEH00N.iptime.org", 
                3092, 
                "project_f_dev", 
                "sql30928530@", 
                database
            ).connectionString;
        }
    }

    public struct WriteHashDataIntoSQL
    {
        string connectionString = null;
        string query = null;
        DynamicParameters parameters = null;

        public WriteHashDataIntoSQL(string key, string value)
        {
            connectionString = new GetDefaultSQLConnection("ProjectF").connectionString;
            query = 
                @$"INSERT INTO ProjectF_UserKey (key, value)
                VALUES (@key, @value)
                ON DUPLICATE KEY UPDATE
                value = VALUES (@value);";

            parameters = new DynamicParameters();
            parameters.Add("key", key, DbType.String, ParameterDirection.Input);
            parameters.Add("value", value, DbType.String, ParameterDirection.Input);
        }

        public async Task ProcessAsync()
        {
            await new SingleMySQLCommand(connectionString, query, parameters).QueryAsync();
        }
    }
}