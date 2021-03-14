using SharpsenStreamBackend.Classes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources.Interfaces
{
    public interface IStreamResource
    {
        Task<StreamDto> getStream(string streamName);
        Task<bool> authenticate(string streamName, string token);
        Task startStream(string streamName);
        Task endStream(string streamName);
        Task<IEnumerable<ChatRoomId>> getChatRooms();
    }
}
