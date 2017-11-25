using System.ComponentModel.DataAnnotations;
using Foodery.Common.Validation.Constants;
using Foodery.Web.Attributes;

namespace Foodery.Web.Models.Auth.Register
{
    public class RegisterRequest
    {
        [Required]
        [StringMinLength(UserConstants.ValidationValues.MinUserNameLength)]
        [StringMaxLength(UserConstants.ValidationValues.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringMinLength(UserConstants.ValidationValues.MinPasswordLength)]
        [StringMaxLength(UserConstants.ValidationValues.MaxPasswordLength)]
        public string Password { get; set; }
    }
}
