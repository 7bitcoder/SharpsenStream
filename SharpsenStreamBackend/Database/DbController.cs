using SharpsenStreamBackend.Classes.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public async Task<int?> Querry(string procedure, SqlParameters parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand command = new SqlCommand(procedure, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Connection.Open();
                SqlParameter retval = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int);
                retval.Direction = System.Data.ParameterDirection.ReturnValue;
                command.Parameters.Add(retval);
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                return (int?)command.Parameters["@ReturnValue"].Value;
            }
        }

        public async Task<T> Querry<T>(string procedure, SqlParameters parameters)
        {
            return (await QuerryList<T>(procedure, parameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<T>> QuerryList<T>(string procedure, SqlParameters parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand command = new SqlCommand(procedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Connection.Open();
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                SqlDataReader dr = await command.ExecuteReaderAsync(CommandBehavior.Default);
                var list = Convert<T>(dr);
                return list;
            }
        }

        public static List<T> Convert<T>(IDataReader dr)
        {
            var list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public async Task<IEnumerable<T>> Execute<T>(string procedure, SqlParameters parameters, Func<T> function)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand command = new SqlCommand(procedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Connection.Open();
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                SqlDataReader dr = await command.ExecuteReaderAsync(CommandBehavior.Default);
                var list = Convert<T>(dr);
                return list;
            }
        }
    }
}
