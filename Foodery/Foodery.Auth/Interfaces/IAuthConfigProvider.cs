using IdentityServer4.Models;
using System.Collections.Generic;

namespace Foodery.Core.Auth.Interfaces
{
    public interface IAuthConfigProvider
    {
        /// <summary>
        /// Get all api resources.
        /// </summary>
        IEnumerable<ApiResource> GetApiResources();

        /// <summary>
        /// Get all identity resources such as Profile etc.
        /// </summary>
        IEnumerable<IdentityResource> GetIdentityResources();

        /// <summary>
        /// Get the clients that can be authenticated.
        /// </summary>
        IEnumerable<Client> GetClients();
    }
}
