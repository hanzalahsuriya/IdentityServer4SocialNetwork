using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace SocialNetwork.oauth.DbMigrations
{
    class Program
    {
        static int Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //      .SetBasePath(Directory.GetCurrentDirectory())
            //      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            //IConfigurationRoot configuration = builder.Build();

            //Console.WriteLine(configuration.GetConnectionString("SocialNetwork"));

            string connectionString = "Server=MARIAHANZALAH\\HANZ;Database=SocialNetwork_IdentityServer_1;Trusted_Connection=True;";

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var upgradeResult = upgrader.PerformUpgrade();
            if (!upgradeResult.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(upgradeResult.Error);
                Console.ResetColor();
                Console.ReadLine();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;

        }
    }
}
