using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Foodery.Auth;
using Foodery.Common.Attributes;
using Foodery.Data;
using Foodery.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foodery.Web.Config
{
    internal static class ServicesExtensions
    {
        private const string DefaultConnectionStringSection = "Default";

        internal static IServiceCollection AddBaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper();
            services.AddOptions();
            services.AddMvc();

            var connectionString = configuration.GetConnectionString(DefaultConnectionStringSection);
            services.AddDbContext<FooderyContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        internal static IServiceCollection AddAuth(this IServiceCollection services)
        {
            var authConfig = new AuthConfigProvider();

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<FooderyContext>()
                    .AddDefaultTokenProviders();
            services.AddIdentityServer()
                    .AddInMemoryApiResources(authConfig.GetApiResources())
                    .AddInMemoryIdentityResources(authConfig.GetIdentityResources())
                    .AddInMemoryClients(authConfig.GetClients())
                    .AddDeveloperSigningCredential(); // This should not be used in production

            return services;
        }

        internal static IServiceCollection AddConventionNamedServices(this IServiceCollection services, IEnumerable<string> assemblyNames)
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

            return services;
        }
    }
}
