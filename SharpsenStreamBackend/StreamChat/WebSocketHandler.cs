using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class WebSocketHandler
    {
        WebSocket _socket;
        byte[] _receiveArray = new byte[512];

        public WebSocketHandler(WebSocket socket)
        {
            _socket = socket;
        }
        public void sendData(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var array = new ArraySegment<byte>(bytes, 0, bytes.Length);
            _socket.SendAsync(array, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public Task<(String data, bool closed)> getData()
        {
            return getString();
        }

        private async Task<(String data, bool closed)> getString()
        {
            WebSocketReceiveResult result;
            var array = new ArraySegment<byte>(_receiveArray);
            using (var ms = new System.IO.MemoryStream())
            {
                do
                {
                    result = await _socket.ReceiveAsync(array, CancellationToken.None);
                    ms.Write(array.Array, array.Offset, result.Count);
                    if (result.CloseStatus.HasValue)
                        return ("", true);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                    return (reader.ReadToEnd(), false);
            }
        }
    }
}
