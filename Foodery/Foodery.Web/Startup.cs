using System.Collections.Generic;
using Foodery.Web.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foodery.Web
{
    public class Startup
    {
        private readonly IEnumerable<string> assemblies;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.assemblies = new[]
            {
                "Foodery.Auth"
            };
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBaseServices(this.configuration)
                    .AddConventionNamedServices(this.assemblies)
                    .AddAuth();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.AddBaseMiddlewares(env);
        }
    }
}
