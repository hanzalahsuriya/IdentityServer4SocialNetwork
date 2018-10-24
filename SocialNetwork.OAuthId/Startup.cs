using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialNetwork.OAuthId.Configuration;
using SocialNetwork.OAuthId.Data;

namespace SocialNetwork.OAuthId
{
    public class Startup
    {
        private ILoggerFactory _logger;
        private IConfiguration _configuration;

        public Startup(ILoggerFactory logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Packages
            // IdentityServer4.AspNetIdentity

            // Migrations
            // dotnet ef migrations add IdentityServerMigration -c ApplicationUserDbContext -o Data/Migrations/IdentityServer/IdentityDb
            // dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
            // dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

            // DB Update 
            // dotnet ef database update -c ApplicationUserDbContext
            // dotnet ef database update -c PersistedGrantDbContext
            // dotnet ef database update -c ConfigurationDbContext

            // Generate Scripts
            // dotnet ef migrations script -c ApplicationUserDbContext -o Data/Scripts/IdentityScript.sql
            // dotnet ef migrations script -c PersistedGrantDbContext -o Data/Scripts/PersistedGrantScript.sql
            // dotnet ef migrations script -c ConfigurationDbContext -o Data/Scripts/ConfigurationScript.sql

            var connectionString = _configuration.GetConnectionString("SocialNetwork");
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationUserDbContext>(opt => opt.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => { })
                .AddEntityFrameworkStores<ApplicationUserDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
             {
                 options.ConfigureDbContext = builder =>
                     builder.UseSqlServer(connectionString,
                         sql => sql.MigrationsAssembly(migrationAssembly));
             })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly));

                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            })
            .AddSigningCredential(new X509Certificate2(@"C:\Users\M.hanzalah\socialnetwork.pfx"));

            //services.AddIdentityServer()
            //    .AddTestUsers(InMemoryConfiguration.Users().ToList())
            //    .AddInMemoryClients(InMemoryConfiguration.Clients())
            //    .AddInMemoryApiResources(InMemoryConfiguration.ApiResources())
            //    .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources())
            //    .AddSigningCredential(new X509Certificate2(@"C:\Users\M.hanzalah\socialnetwork.pfx"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.AddConsole();
            app.UseDeveloperExceptionPage();
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
