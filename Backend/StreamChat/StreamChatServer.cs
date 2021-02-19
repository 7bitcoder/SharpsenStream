

using SharpsenStreamBackend.Resources;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class StreamChatServer
    {
        ChatRooms _chatRooms;
        IUserResource _userResource;
        public StreamChatServer(ChatRooms chatRooms, IUserResource userResource)
        {
            _chatRooms = chatRooms;
            _userResource = userResource;
        }
        public async void handleUser(WebSocket webSocket, TaskCompletionSource<object> end)
        {
            try
            {
                var user = new ChatMember(webSocket);
                var data = await waitForData(user);
                var userData = await _userResource.getUser(data.userId);
                user.init(userData);
                await _chatRooms.handleUser(user, data.chatId);

            }
            catch (Exception) { }
            finally
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
                end.TrySetResult(new object());
            }
        }

        private async Task<(int chatId, int userId)> waitForData(ChatMember member)
        {
            var message = await member.getMessage();
            var id = message.message.Split(";");
            int chatId = Int32.Parse(id[0]);
            int userId = Int32.Parse(id[1]);
            return (chatId, userId);
        }
    }
}
