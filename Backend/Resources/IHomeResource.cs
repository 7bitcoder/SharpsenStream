using SharpsenStreamBackend.Classes.Dto;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public interface IHomeResource
    {
        Task<User> Login(string userName, string password);
        Task<bool> authenticate(string streamName, string token);
        Task<bool> logOut(int userId);
    }
}
