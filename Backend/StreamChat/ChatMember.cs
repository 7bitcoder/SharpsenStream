using SharpsenStreamBackend.Classes;
using SharpsenStreamBackend.Classes.Dto;
using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class UserDisconnected : Exception { }
    public class ChatMember
    {
        static int cnt = 0;
        User data;

        WebSocketHandler _socketHandler;
        public ChatMember(WebSocket socket)
        {
            cnt++;
            _socketHandler = new WebSocketHandler(socket);
        }
        public void init(User user)
        {
            this.data = user;
        }
        public void sendMessage(Message message)
        {
            var rawMessage = JsonSerializer.Serialize(message);
            _socketHandler.sendData(rawMessage);
        }

        public async Task<Message> getMessage()
        {
            var data = await _socketHandler.getData();
            if (data.closed)
            {
                throw new UserDisconnected();
            }
            var message = JsonSerializer.Deserialize<Message>(data.data);
            message.userName = this.data?.Username;
            message.color = this.data?.Color ?? 0;
            return message;
        }
    }
}
