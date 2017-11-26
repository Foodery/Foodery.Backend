namespace Foodery.Web.Models
{
    /// <summary>
    /// Represents error data for given field.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The field that has errors.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Error message for the field.
        /// </summary>
        public string Message { get; set; }
    }
}
