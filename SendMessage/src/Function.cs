using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Runtime;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SendMessage
{
    public class Function
    {
        private ServiceProvider _service;
        private ConnectionSocketService _connectionService;

        public Function()
            : this(Bootstrap.CreateInstance()) { }

        public Function(ServiceProvider service)
        {
            _service = service;
            _connectionService = _service.GetService<ConnectionSocketService>();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var domainName = request.RequestContext.DomainName;
                var stage = request.RequestContext.Stage;
                var endpoint = $"https://{domainName}/{stage}";
                context.Logger.LogLine($"API Gateway management endpoint: {endpoint}");

                var message = JsonConvert.DeserializeObject<JObject>(request.Body);
                var data = message["data"]?.ToString();
                var connection_id = message["connection_id"]?.ToString();
                var channel = message["channel"]?.ToString();

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

                var apiClient = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig
                {
                    ServiceURL = endpoint
                });

                bool post_multiple = false;
                bool post_single = false;
                List<ConnectionSocketModel> list_connections = new List<ConnectionSocketModel>();
                ConnectionSocketModel connection = new ConnectionSocketModel();

                if (CheckJObjectKeyValue(connection_id) == false && CheckJObjectKeyValue(channel) == false)
                {
                    context.Logger.LogLine($"Not have connection_id and channel");
                    list_connections = await _connectionService.ListConnection();
                    post_multiple = true;
                }
                else if (CheckJObjectKeyValue(connection_id) && CheckJObjectKeyValue(channel))
                {
                    context.Logger.LogLine($"Have connection_id as: {connection_id}, and channel as: {channel}");
                    connection = await _connectionService.SendToConnectionChannel(connection_id, channel);
                    post_single = true;
                }
                else if (CheckJObjectKeyValue(channel) && CheckJObjectKeyValue(connection_id) == false)
                {
                    context.Logger.LogLine($"Have only channel as: {channel}");
                    list_connections = await _connectionService.ListConnectionInChannel(channel);
                    post_multiple = true;
                }
                else if (CheckJObjectKeyValue(connection_id) && CheckJObjectKeyValue(channel) == false)
                {
                    context.Logger.LogLine($"Have only connection_id as: {connection_id}");
                    list_connections = await _connectionService.SendToConnection(connection_id);
                    post_multiple = true;
                }

                if (post_single)
                {
                    if (connection != null)
                    {
                        context.Logger.LogLine($"Post to single connection {connection.connection_id}");
                        await PostToConnection(connection, stream, _connectionService, apiClient, context);
                    }
                }

                if (post_multiple)
                {
                    if (list_connections.Any())
                    {
                        foreach (var connection_item in list_connections)
                        {
                            context.Logger.LogLine($"Post to multiple connection {connection_item.connection_id}");
                            await PostToConnection(connection_item, stream, _connectionService, apiClient, context);
                        }
                    }
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = "Data sent"
                };
            }
            catch (Exception e)
            {
                context.Logger.LogLine("Error disconnecting: " + e.Message);
                context.Logger.LogLine(e.StackTrace);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Failed to send message: {e.Message}"
                };
            }
        }

        public async Task PostToConnection(ConnectionSocketModel connection, MemoryStream stream, ConnectionSocketService connectionService, AmazonApiGatewayManagementApiClient apiClient, ILambdaContext context)
        {
            var connectionId = connection.connection_id;
            context.Logger.LogLine($"Get Connection ID from DB: {connectionId}");

            var postConnectionRequest = new PostToConnectionRequest
            {
                ConnectionId = connectionId,
                Data = stream
            };

            try
            {
                context.Logger.LogLine($"Post to connection: {connectionId}");
                stream.Position = 0;
                await apiClient.PostToConnectionAsync(postConnectionRequest);
            }
            catch (AmazonServiceException e)
            {
                if (e.StatusCode == HttpStatusCode.Gone)
                {
                    connectionService.GetConnection(connectionId);
                    context.Logger.LogLine($"Deleting gone connection: {connectionId}");
                }
                else
                {
                    context.Logger.LogLine($"Error posting message to {connectionId}: {e.Message}");
                    context.Logger.LogLine(e.StackTrace);
                }
            }
        }

        public bool CheckJObjectKeyValue(string key_name)
        {
            if (string.IsNullOrEmpty(key_name))
            {
                return false;
            }

            return true;
        }
    }
}
