using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Foodery.Web.Config
{
    internal static class MiddlewareExtensions
    {
        internal static void AddBaseMiddlewares(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
