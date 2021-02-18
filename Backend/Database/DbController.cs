using Dapper;
using SharpsenStreamBackend.Classes.Dto;
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
        private DbSettings _dbSettings;
        private SqlConnectionStringBuilder _builder;
        private string _connectionStr;

        public DbController()
        {
            using StreamReader file = File.OpenText(@"dbSettings.json");
            _dbSettings = JsonSerializer.Deserialize<DbSettings>(file.ReadToEnd());
            buildConnectionString();
        }

        private void buildConnectionString()
        {
            _builder = new SqlConnectionStringBuilder()
            {
                DataSource = _dbSettings.Machiene,
                InitialCatalog = _dbSettings.DatabaseName,
                UserID = $@"{_dbSettings.Machiene}/{_dbSettings.User}",
                IntegratedSecurity = true
            };
            _connectionStr = _builder.ConnectionString;
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
