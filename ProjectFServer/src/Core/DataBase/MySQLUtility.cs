using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace ProjectF.Networks.DataBases
{
    public class MySQLProcessBase
    {
        private string connectionString = null;

        public MySQLProcessBase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }

    public class SingleMySQLCommand : MySQLProcessBase
    {
        private string query = null;
        private DynamicParameters parameters;

        public SingleMySQLCommand(string connectionString, string query, DynamicParameters parameters) : base(connectionString)
        {
            this.query = query;
            this.parameters = parameters;
        }

        public async Task QueryAsync()
        {
            try {
                using(IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    try {
                        using (IDbTransaction dbTransaction = connection.BeginTransaction())
                        {
                            await connection.ExecuteAsync(query, parameters, dbTransaction);
                            dbTransaction.Commit();
                        }
                    } catch(Exception _) {
                        
                    } finally {
                        connection.Close();
                    }
                }
            } catch(Exception _) {

            }
        }
    }

    public class SingleMySQLQuery : MySQLProcessBase
    {
        private string query = null;
        private DynamicParameters parameters;

        public SingleMySQLQuery(string connectionString, string query, DynamicParameters parameters) : base(connectionString)
        {
            this.query = query;
            this.parameters = parameters;
        }

        public async Task<List<T>> QueryAsync<T>()
        {
            List<T> resultData = null;

            try {
                using(IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    try {
                        using (IDbTransaction dbTransaction = connection.BeginTransaction())
                        {
                            resultData = (await connection.QueryAsync<T>(query, parameters, dbTransaction)).ToList();
                            dbTransaction.Commit();
                        }
                    } catch(Exception _) {
                        return null;
                    } finally {
                        connection.Close();
                    }
                }
            } catch(Exception _) {
                return null;
            }

            return resultData;
        }
    }
}