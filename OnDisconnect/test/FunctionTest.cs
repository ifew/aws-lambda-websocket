using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using OnDisconnect;
using System.IO;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OnDisconnect.Tests
{
    public class FunctionTest
    {
        //[Fact]
        public void IntegrationTestPostMethod()
        {
            var requestString = File.ReadAllText("./SampleRequests/TestPostMethod.json");

            TestLambdaContext context;
            APIGatewayProxyRequest request;
            Task<APIGatewayProxyResponse> response;

            Function functions = new Function();
            
            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.FunctionHandler(request, context);

            Assert.Equal(200, response.Result.StatusCode);
            Assert.Equal("Connected", response.Result.Body);
        }
        
        [Fact]
        public void TestPostMethod()
        {
            var requestString = File.ReadAllText("./SampleRequests/TestPostMethod.json");

            TestLambdaContext context;
            APIGatewayProxyRequest request;
            Task<APIGatewayProxyResponse> response;

            var provider = new ServiceCollection()
            .AddDbContext<FunctionContext>(options => options.UseInMemoryDatabase("delete_connection"))
            .AddSingleton<ConnectionSocketService, ConnectionSocketService>()
            .BuildServiceProvider();

            FunctionContext db_add_context = provider.GetRequiredService<FunctionContext>();
            db_add_context.Connections.Add(new ConnectionSocketModel { 
                id = 1,
                connection_id = "xCKJA1233=",
                user_id = "900123456",
                channel = "aFc34gxe9v"
            });
            db_add_context.SaveChanges();

            Function functions = new Function(provider);

            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.FunctionHandler(request, context);
            
            Assert.Equal(200, response.Result.StatusCode);
            Assert.Equal("Disconnected", response.Result.Body);
        }

        [Fact]
        public void TestService()
        {
            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("delete_connection_service").Options;
            FunctionContext db_context = new FunctionContext(_options);
            using (FunctionContext db_add_context = new FunctionContext(_options))
            {
                db_add_context.Connections.Add(new ConnectionSocketModel { 
                    id = 1,
                    connection_id = "xCKJA1233=",
                    user_id = "900123456",
                    channel = "aFc34gxe9v"
                });
                db_add_context.SaveChanges();
            }

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            var response = service.DeleteConnection("xCKJA1233=");
            
            Assert.Equal(200, response.Result.StatusCode);
            Assert.Equal("Disconnected", response.Result.Body);
        }
    }
}
