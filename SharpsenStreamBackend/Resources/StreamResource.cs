using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Database;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public class StreamResource : IStreamResource
    {
        private DbController _dbController;
        public StreamResource(DbController dbController)
        {
            this._dbController = dbController;
        }

        public async Task<bool> authenticate(string streamName, string token)
        {
            var parameters = new SqlParameters();
            parameters.Add("@StreamName", SqlDbType.VarChar, streamName, 256);
            parameters.Add("@Token", SqlDbType.VarChar, token, 512);
            var res = await _dbController.Querry("dbo.AuthenticateStreamInit", parameters);
            return res == 1;
        }

        public Task<IEnumerable<ChatRoomId>> getChatRooms()
        {
            return _dbController.QuerryList<ChatRoomId>("dbo.GetChatRooms", new SqlParameters());
        }

        public async Task<StreamDto> getStream(string streamName)
        {
            var parameters = new SqlParameters();
            parameters.Add("@StreamName", SqlDbType.VarChar, streamName, 256);
            var res = await _dbController.Querry<StreamDto>("dbo.GetStream", parameters);
            return res;
        }
    }
}
