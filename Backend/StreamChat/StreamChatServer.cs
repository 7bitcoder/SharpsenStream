

using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Resources;
using SharpsenStreamBackend.Resources.Interfaces;
using System;
using System.Drawing;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class StreamChatServer
    {
        private readonly Random _random = new Random();
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
                var userData = await getUser(data.userId);
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

        private async Task<User> getUser(int id)
        {
            User user = null;
            if (id > 0)
            {
                user = await _userResource.getUser(id);
            }
            else
            {
                user = new User
                {
                    Username = "anon" + _random.Next(1000, 9999),
                    Color = Color.White.ToArgb(), // white 16777215
                    AvatarFilePath = "/",
                    Email = "",
                    UserId = 0
                };
            }
            return user;
        }
    }
}
