using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using OnConnect;
using System.IO;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OnConnect.Tests
{
    public class FunctionTest
    {
        [Fact]
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
            .AddDbContext<FunctionContext>(options => options.UseInMemoryDatabase("add_connection"))
            .AddSingleton<ConnectionSocketService, ConnectionSocketService>()
            .BuildServiceProvider();

            Function functions = new Function(provider);

            request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestString);
            context = new TestLambdaContext();
            response = functions.FunctionHandler(request, context);
            
            Assert.Equal(200, response.Result.StatusCode);
            Assert.Equal("Connected", response.Result.Body);
        }

        [Fact]
        public void TestService()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("add_connection_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            var data = new ConnectionSocketModel { 
                id = 1,
                connection_id = "xCKJA1233=",
                user_id = "900123456",
                channel = "aFc34gxe9v"
            };

            var response = service.AddConnection(data);
            
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Connected", response.Body);
        }
    }
}
