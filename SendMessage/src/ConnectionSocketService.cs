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

        public List<ConnectionSocketModel> ListConnection()
        {
            List<ConnectionSocketModel> data_list = _context_db.Connections.ToList();

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

        public List<ConnectionSocketModel> ListConnectionInChannel(string channel) {
            List<ConnectionSocketModel> data_list = _context_db.Connections.Where(c => c.channel == channel).ToList();
            return data_list;
        }

        public List<ConnectionSocketModel> SendToConnection(string connection_id) {
            List<ConnectionSocketModel> data_list = _context_db.Connections.Where(c => c.connection_id == connection_id).ToList();
            return data_list;
        }

        public ConnectionSocketModel SendToConnectionChannel(string connection_id, string channel)
        {
            ConnectionSocketModel data = _context_db.Connections.Where(c => c.connection_id == connection_id && c.channel == channel).FirstOrDefault();

            if(data.connection_id == null)
            {
                return new ConnectionSocketModel();
            }

            return data;
        }
    }
}