using Dapper;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Database;
using System.Data;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public class UserResource : IUserResource
    {
        private DbController _dbController;
        public UserResource(DbController dbController)
        {
            this._dbController = dbController;
        }
        public Task<User> getUser(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            return this._dbController.Querry<User>("dbo.GetUser", parameters);
        }
    }
}
