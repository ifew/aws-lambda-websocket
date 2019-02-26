using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SendMessage
{
    public class Bootstrap
    {
        
        public static ServiceProvider CreateInstance()
        {
            return new ServiceCollection()
            .AddDbContext<FunctionContext>(options => options.UseMySQL(LambdaConfiguration.Instance["TEST_LAMBDA_DBCONNECTION"]))
            .AddSingleton<ConnectionSocketService, ConnectionSocketService>()
            .BuildServiceProvider();
        }
    }
}