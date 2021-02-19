using SharpsenStreamBackend.Classes.Dto;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public interface IUserResource
    {
        Task<User> getUser(int id);
    }
}
