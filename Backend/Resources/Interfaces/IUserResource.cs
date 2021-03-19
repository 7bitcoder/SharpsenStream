using SharpsenStreamBackend.Classes;
using System;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources.Interfaces
{
    public interface IUserResource
    {
        Task<User> Login(string userName, string password);
        Task<bool> logOut(int userId);
        Task<User> getUser(int id);
        Task setToken(int id, string token, DateTime expiration);
        Task setRefreshToken(int id, string token, DateTime expiration);
        Task deleteToken(int userId, string token);
    }
}
