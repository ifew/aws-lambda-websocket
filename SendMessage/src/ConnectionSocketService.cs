using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace SendMessage
{
    public class ConnectionSocketService
    {
        private readonly FunctionContext _context_db;
        public ConnectionSocketService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public async Task<List<ConnectionSocketModel>> ListConnection()
        {
            List<ConnectionSocketModel> data_list = await _context_db.Connections.ToListAsync();

            return data_list;
        }

        public ConnectionSocketModel GetConnection(string connection_id)
        {
            ConnectionSocketModel data = _context_db.Connections.Where(c => c.connection_id == connection_id).FirstOrDefault();

            return data;
        }

        public APIGatewayProxyResponse DeleteConnection(string connection_id)
        {
            _context_db.Remove(_context_db.Connections.Single(a => a.connection_id == connection_id));
            var save_result = _context_db.SaveChanges();

            APIGatewayProxyResponse respond = new APIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                },
                Body = "Disconnected"
            };

            return respond;
        }

        public async Task<List<ConnectionSocketModel>> ListConnectionInChannel(string channel) {
            List<ConnectionSocketModel> data_list = await _context_db.Connections.Where(c => c.channel == channel).ToListAsync();
            return data_list;
        }

        public async Task<List<ConnectionSocketModel>> SendToConnection(string connection_id) {
            List<ConnectionSocketModel> data_list = await _context_db.Connections.Where(c => c.connection_id == connection_id).ToListAsync();
            return data_list;
        }

        public async Task<ConnectionSocketModel>SendToConnectionChannel(string connection_id, string channel)
        {
            var isChannel = false;
            var isConnection = false;

            if (!String.IsNullOrEmpty(channel))
                isChannel = true;

            if (!String.IsNullOrEmpty(connection_id))
                isConnection = true;

            if (isConnection && isChannel)
            {
                ConnectionSocketModel data = await _context_db.Connections.Where(c => c.connection_id == connection_id && c.channel == channel).FirstOrDefaultAsync();
                return data;
            }

            return new ConnectionSocketModel();


        }
    }
}