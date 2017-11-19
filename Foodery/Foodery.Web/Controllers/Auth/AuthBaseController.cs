﻿using Microsoft.AspNetCore.Mvc;

namespace Foodery.Web.Controllers.Auth
{
    public class AuthBaseController : BaseController
    {
        protected const string InvalidUserNameOrPassword = "Invalid username or password provided.";
        protected const string UserAlreadyExists = "Such user already exists.";
    }
}
