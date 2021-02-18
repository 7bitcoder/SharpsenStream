using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Resources;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class ChatRooms
    {

        IStreamResource _streamResource;
        ConcurrentDictionary<int, ChatRoom> _chatRooms = new ConcurrentDictionary<int, ChatRoom>();
        public ChatRooms(IStreamResource streamResource)
        {
            _streamResource = streamResource;
            _streamResource.getChatRooms().ContinueWith(async chatIds => makeChatRooms(await chatIds));
        }

        private void makeChatRooms(IEnumerable<ChatRoomId> chatIds)
        {
            foreach (var id in chatIds)
            {
                _chatRooms.TryAdd(id.ChatId, new ChatRoom(id.ChatId));
            }
        }

        public void makeNewChatRoom(int chatId)
        {
            _chatRooms.TryAdd(chatId, new ChatRoom(chatId));
        }

        public async Task handleUser(ChatMember member, int chatId)
        {
            if (_chatRooms.TryGetValue(chatId, out ChatRoom room))
            {
                await room.handleUser(member);
            }
        }
    }
}
