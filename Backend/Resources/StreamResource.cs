using Dapper;
using SharpsenStreamBackend.Classes.Dto;
using SharpsenStreamBackend.Database;
using SharpsenStreamBackend.Resources.Interfaces;
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

        public Task<IEnumerable<ChatRoomId>> getChatRooms()
        {
            return _dbController.QuerryList<ChatRoomId>("dbo.GetChatRooms", new DynamicParameters());
        }

        public async Task<StreamDto> getStream(string streamName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StreamName", streamName, DbType.String);
            var res = await _dbController.Querry<StreamDto>("dbo.GetStream", parameters);
            return res;
        }


        public async Task<bool> authenticate(string streamName, string token)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StreamName", streamName, DbType.String);
            parameters.Add("@Token", token, DbType.String);
            var res = await _dbController.Querry("dbo.AuthenticateStreamInit", parameters);
            return res == 1;
        }

        public Task startStream(string streamName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StreamName", streamName, DbType.String);
            return _dbController.Querry("sbo.SetStreamStartTime", parameters);
        }

        public Task endStream(string streamName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StreamName", streamName, DbType.String);
            return _dbController.Querry("sbo.EndStream", parameters);
        }
    }
}
