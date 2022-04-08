using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Configuration;

namespace CosmosAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
.ConfigureAppConfiguration((context, config) =>
{
    //var options = new DefaultAzureCredentialOptions { ExcludeVisualStudioCredential = true };
    //var client = new SecretClient(new Uri(Environment.GetEnvironmentVariable("VaultUri")), new DefaultAzureCredential(options));
    //string userAssignedClientId = Environment.GetEnvironmentVariable("VaultID");
    var kvUri = Environment.GetEnvironmentVariable("VaultUri");
    var IdentityClientID = Environment.GetEnvironmentVariable("ManagedIdentityClientId");
    var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = IdentityClientID });
    config.AddAzureKeyVault(
            new Uri(kvUri),
            credential);
})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
