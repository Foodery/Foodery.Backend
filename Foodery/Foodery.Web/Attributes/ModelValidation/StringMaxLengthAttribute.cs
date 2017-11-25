using System;
using System.ComponentModel.DataAnnotations;
using Foodery.Common.Validation.Constants;

namespace Foodery.Web.Attributes
{
    /// <summary>
    /// Validates that the string has given maximum length.
    /// </summary>
    public class StringMaxLengthAttribute : MaxLengthAttribute
    {
        public StringMaxLengthAttribute(int length) 
            : base(length)
        {
        }

        /// <summary>
        /// Check if the given value is valid.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Flag that indicates if the value has the specified max length.</returns>
        public override bool IsValid(object value)
        {
            if (!(value is string))
            {
                throw new InvalidOperationException(TypeConstants.ValidationMessages.NotAStringMessage);
            }

            return base.IsValid(value);
        }

        /// <summary>
        /// Creates a message that will be used in case of error.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns>Formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return $"{name} should be maximum {this.Length} charaters long.";
        }
    }
}
