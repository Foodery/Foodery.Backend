﻿using System;
using System.Collections.Generic;
using Foodery.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Foodery.Auth.Interfaces;

namespace Foodery.Auth
{
    public class ApplicationUserManager : UserManager<User>, IApplicationUserManager
    {
        public ApplicationUserManager(IUserStore<User> store, 
                                      IOptions<IdentityOptions> optionsAccessor, 
                                      IPasswordHasher<User> passwordHasher, 
                                      IEnumerable<IUserValidator<User>> userValidators, 
                                      IEnumerable<IPasswordValidator<User>> passwordValidators, 
                                      ILookupNormalizer keyNormalizer, 
                                      IdentityErrorDescriber errors, 
                                      IServiceProvider services, 
                                      ILogger<UserManager<User>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
