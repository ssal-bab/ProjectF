using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace ProjectF.Networks.DataBases
{
    public class MySQLProcessBase
    {
        private string connectionString = null;

        public ENetworkResult Result = ENetworkResult.None;
        public string Reason = null;

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
        private string storedProcedure = null;
        private DynamicParameters parameters;

        public SingleMySQLCommand(string connectionString, string storedProcedure, DynamicParameters parameters) : base(connectionString)
        {
            this.storedProcedure = storedProcedure;
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
                            await connection.ExecuteAsync(storedProcedure, parameters, dbTransaction, commandType: CommandType.StoredProcedure);
                            dbTransaction.Commit();
                            Result = ENetworkResult.Success;
                        }
                    } catch(Exception err) {
                        Result = ENetworkResult.DBError;
                        Reason = err.Message;
                    } finally {
                        connection.Close();
                    }
                }
            } catch(Exception err) {
                Result = ENetworkResult.DBError;
                Reason = err.Message;
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
                            resultData = (await connection.QueryAsync<T>(query, parameters, dbTransaction, commandType: CommandType.StoredProcedure)).ToList();
                            dbTransaction.Commit();
                            Result = ENetworkResult.Success;
                        }
                    } catch(Exception err) {
                        Result = ENetworkResult.DBError;
                        Reason = err.Message;
                        return null;
                    } finally {
                        connection.Close();
                    }
                }
            } catch(Exception err) {
                Result = ENetworkResult.DBError;
                Reason = err.Message;
                return null;
            }

            return resultData;
        }
    }
}