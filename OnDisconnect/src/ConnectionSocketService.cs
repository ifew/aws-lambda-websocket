using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace OnDisconnect
{
    public class ConnectionSocketService
    {
        private readonly FunctionContext _context_db;
        public ConnectionSocketService(FunctionContext context_db) {
            _context_db = context_db;
        }

        public async Task<APIGatewayProxyResponse> DeleteConnection(string connection_id)
        {
            try {
                _context_db.Remove(_context_db.Connections.SingleOrDefault(a => a.connection_id == connection_id));
                _context_db.SaveChanges();

                APIGatewayProxyResponse respond = new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.OK,
                    Headers = new Dictionary<string, string>
                    { 
                        { "Content-Type", "application/json" }, 
                        { "Access-Control-Allow-Origin", "*" } 
                    },
                    Body = "Disconnected"
                };

                return await Task.FromResult(respond);
            } 
            catch(Exception e) {
                System.Console.WriteLine(e.Message);
                return new APIGatewayProxyResponse {
                    StatusCode = 500,
                    Body = $"Service Fail: {e.Message}"
                };
            }
        }
    }
}