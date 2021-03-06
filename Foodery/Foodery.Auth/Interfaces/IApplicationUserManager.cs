﻿using Foodery.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Foodery.Auth.Interfaces
{
    public interface IApplicationUserManager
    {
        /// <summary>
        /// Hashing utilities service.
        /// </summary>
        IPasswordHasher<User> PasswordHasher { get;  }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="user">User data.</param>
        Task<IdentityResult> CreateAsync(User user);

        /// <summary>
        /// Find user by given username.
        /// </summary>
        /// <param name="userName">The name of the user, by which the search will be performed.</param>
        /// <returns>Found user.</returns>
        Task<User> FindByNameAsync(string userName);

        /// <summary>
        /// Verify that the given password matches the user's one.
        /// </summary>
        /// <param name="user">User to which the password will be tested.</param>
        /// <param name="password">Password to test.</param>
        /// <returns>Flag that indicates if the password matches.</returns>
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
