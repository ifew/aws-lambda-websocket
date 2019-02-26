using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OnDisconnect
{
    public static class LambdaConfiguration
    {
        private static IConfigurationRoot instance = null;
        public static IConfigurationRoot Instance
        {
            get 
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var basePath = Directory.GetCurrentDirectory();

                return instance ?? (instance = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables()
                    .Build());
            }
        }
    }
}