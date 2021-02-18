using SharpsenStreamBackend.Classes;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class UserDisconnected : Exception { }
    public class ChatMember
    {
        static int cnt = 0;
        string userName;

        WebSocketHandler _socketHandler;
        public ChatMember(WebSocket socket)
        {
            cnt++;
            userName = String.Concat(Enumerable.Repeat(cnt.ToString(), 8));
            _socketHandler = new WebSocketHandler(socket);
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
            message.userName = userName;
            return message;
        }
    }
}
