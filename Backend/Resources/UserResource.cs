using Dapper;
using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Database;
using System.Data;
using System.Security.Cryptography;
using System.Text;
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

        public Task<User> Login(string userName, string password)
        {
            string hash;
            using (SHA256 sha256Hash = SHA256.Create())
                hash = GetHash(sha256Hash, password);
            var parameters = new DynamicParameters();
            parameters.Add("@Username", userName, DbType.String);
            parameters.Add("@PasswordHash", hash, DbType.String);
            return _dbController.Querry<User>("dbo.Login", parameters);
        }

        public Task<bool> logOut(int userId)
        {
            throw new System.NotImplementedException();
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public Task<UserToken> getNewUserToken(int id, string oldToken = null, int? daysOffset = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", id, DbType.Int32);
            if (oldToken != null)
            {
                parameters.Add("@OldToken", oldToken, DbType.String);
            }
            if (daysOffset.HasValue)
            {
                parameters.Add("@ExpireDays", daysOffset.Value, DbType.Int32);
            }
            return _dbController.Querry<UserToken>("dbo.GetNewToken", parameters);
        }

        public async Task deleteToken(int userId, string token)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.Int32);
            parameters.Add("@Token", token, DbType.String);
            await _dbController.Querry("dbo.DeleteToken", parameters);
            return;
        }
    }
}
