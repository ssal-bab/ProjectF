using System.Data;
using Dapper;
using Newtonsoft.Json;
using ProjectF.Datas;

namespace ProjectF.Networks.DataBases
{
    public struct CreateMySQLConnection
    {
        public string connectionString = null;

        public CreateMySQLConnection(string address, int port, string userID, string password, string database)
        {
            connectionString = $"server={address};port={port};uid={userID};password={password};database={database}";
        }
    }

    public struct CreateGameMySQLConnection
    {
        public string connectionString = null;

        public CreateGameMySQLConnection()
        {
            // 우선은 하드코딩
            connectionString = new CreateMySQLConnection(
                "SEH00N.iptime.org", 
                3092, 
                "project_f_dev", 
                "sql30928530@", 
                "ProjectF"
            ).connectionString;
        }
    }
}