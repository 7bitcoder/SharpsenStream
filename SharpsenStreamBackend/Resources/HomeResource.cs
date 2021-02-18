using SharpsenStreamBackend.Database;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public class HomeResource : IHomeResource
    {
        private DbController _dbController;
        public HomeResource(DbController dbController)
        {
            this._dbController = dbController;
        }

        public async Task<bool> authenticate(string streamName, string token)
        {
            var parameters = new SqlParameters();
            parameters.Add("@StreamName", SqlDbType.VarChar, streamName, 256);
            parameters.Add("@Token", SqlDbType.VarChar, token, 512);
            var res = await _dbController.Querry("dbo.AuthenticateStreamInit", parameters);
            return res == 1;
        }

        public async Task<bool> Login(string userName, string password)
        {
            string hash;
            using (SHA256 sha256Hash = SHA256.Create())
                hash = GetHash(sha256Hash, password);
            var parameters = new SqlParameters();
            parameters.Add("@Username", SqlDbType.VarChar, userName, 256);
            parameters.Add("@PasswordHash", SqlDbType.VarChar, hash, 256);
            var res = await _dbController.Querry("dbo.Login", parameters);
            return res == 1;
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
    }
}
