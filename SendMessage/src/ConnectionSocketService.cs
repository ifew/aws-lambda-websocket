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

        public List<ConnectionSocketModel> ListConnection(string connection_id)
        {
            //List<ConnectionSocketModel> data_list = _context_db.Connections.Where(p => p.connection_id == connection_id).ToList();
            List<ConnectionSocketModel> data_list = _context_db.Connections.ToList();

            return data_list;
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
    }
}