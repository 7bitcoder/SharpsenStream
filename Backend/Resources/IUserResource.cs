using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Classes.Dto;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public interface IUserResource
    {
        Task<User> Login(string userName, string password);
        Task<bool> logOut(int userId);
        Task<User> getUser(int id);
        Task<UserToken> getNewUserToken(int id, string oldToken = null, int? daysOffset = null);
        Task deleteToken(int userId, string token);
    }
}
