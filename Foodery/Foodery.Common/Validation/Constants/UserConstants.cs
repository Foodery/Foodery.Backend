namespace Foodery.Common.Validation.Constants
{
    public class UserConstants
    {
        public static class ValidationMessages
        {
            public const string InvalidUserNameOrPassword = "Invalid username or password provided.";
            public const string UserAlreadyExists = "Such usser already exists.";

            public const string RegistrationFailed = "Registration failed.";
            public const string LoginFailed = "Login failed.";
        }

        public static class ValidationValues
        {
            public const int MinUserNameLength = 4;
            public const int MaxUserNameLength = 30;

            public const int MinPasswordLength = 8;
            public const int MaxPasswordLength = 40;
        }
    }
}
