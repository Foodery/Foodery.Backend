using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Foodery.Web.Config
{
    internal static class MiddlewareExtensions
    {
        internal static IApplicationBuilder AddBaseMiddlewares(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin(); // For testing purposes
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });

            return app;
        }
    }
}
