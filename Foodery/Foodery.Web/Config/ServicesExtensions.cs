using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foodery.Common.Attributes;
using Foodery.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Foodery.Web.Config
{
    internal static class ServicesExtensions
    {
        private const string DefaultConnectionStringSection = "Default";

        internal static void AddBaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddMvc();

            var connectionString = configuration.GetConnectionString(DefaultConnectionStringSection);
            services.AddDbContext<FooderyContext>(options => options.UseSqlServer(connectionString));
        }

        internal static void AddConventionNamedServices(this IServiceCollection services, IEnumerable<string> assemblyNames)
        {
            foreach (var name in assemblyNames)
            {
                var assembly = Assembly.Load(name);
                var types = assembly.GetTypes().Where(t => t.IsClass);

                foreach (var classType in types)
                {
                    var defaultInterface = classType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{classType.Name}");

                    if (defaultInterface != null)
                    {
                        var singletonAttribute = Attribute.GetCustomAttribute(classType, typeof(SingletonBindingAttribute));

                        if (singletonAttribute != null)
                        {
                            services.AddSingleton(defaultInterface, classType);
                        }
                        else
                        {
                            services.AddScoped(defaultInterface, classType);
                        }
                    }
                }
            }
        }
    }
}
