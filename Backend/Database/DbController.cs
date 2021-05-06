using Dapper;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Resources;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Database
{
    public class DbController
    {
        private string _connectionStr;

        public DbController(Config config)
        {
            buildConnectionString(config);
        }

        private void buildConnectionString(Config config)
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = config.config.database.Machiene,
                InitialCatalog = config.config.database.DatabaseName,
                UserID = $@"{config.config.database.Machiene}/{config.config.database.User}",
                IntegratedSecurity = true
            };
            _connectionStr = builder.ConnectionString;
        }

        public async Task Run(string procedure, DynamicParameters parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionStr))
            {
                await connection.ExecuteAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<int?> Querry(string procedure, DynamicParameters parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionStr))
            {
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int?>("@ReturnValue");
            }
        }

        public async Task<T> Querry<T>(string procedure, DynamicParameters parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionStr))
            {
                var result = await connection.QueryFirstAsync<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<IEnumerable<T>> QuerryList<T>(string procedure, DynamicParameters parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionStr))
            {
                var result = await connection.QueryAsync<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
