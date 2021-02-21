using SharpsenStreamBackend.Classes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public interface IStreamResource
    {
        Task<StreamDto> getStream(string streamName);
        Task<bool> authenticate(string streamName, string token);
        Task<IEnumerable<ChatRoomId>> getChatRooms();
    }
}
