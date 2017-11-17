using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foodery.Web.Config;

namespace Foodery.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var assemblyNames = new [] 
            {
                "Foodery.Data"
            };

            services.AddBaseServices(this.Configuration);
            services.AddConventionNamedServices(assemblyNames);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.AddBaseMiddlewares(env);
        }
    }
}
