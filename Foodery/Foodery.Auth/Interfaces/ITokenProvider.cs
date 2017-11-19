using System.Threading.Tasks;
using Foodery.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Foodery.Core.Auth.Interfaces
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Setup and create JWT by given user data.
        /// </summary>
        /// <param name="user">User data used for the generation of JWT.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>JWT.</returns>
        Task<string> GetJWT(User user, HttpContext httpContext);
    }
}
