using Foodery.Web.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddBaseServices(this.Configuration)
                    .AddConventionNamedServices(null)
                    .AddAuth();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.AddBaseMiddlewares(env);
        }
    }
}
