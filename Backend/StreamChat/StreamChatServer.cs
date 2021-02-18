

using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class StreamChatServer
    {
        ChatRooms _chatRooms;
        public StreamChatServer(ChatRooms chatRooms)
        {
            _chatRooms = chatRooms;
        }
        public async void handleUser(WebSocket webSocket, TaskCompletionSource<object> end)
        {
            try
            {
                var user = new ChatMember(webSocket);
                var chatId = await waitForChatId(user);
                await _chatRooms.handleUser(user, chatId);

            }
            catch (Exception e)
            {

            }
            finally
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
                end.TrySetResult(new object());
            }
        }

        private async Task<int> waitForChatId(ChatMember member)
        {
            var message = await member.getMessage();
            int chatId = Int32.Parse(message.message);
            return chatId;
        }
    }
}
