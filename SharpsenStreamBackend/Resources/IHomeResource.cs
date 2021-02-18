using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public interface IHomeResource
    {
        Task<bool> Login(string userName, string password);
        Task<bool> authenticate(string streamName, string token);
        Task<bool> logOut(int userId);
    }
}
