using Newtonsoft.Json;

namespace Foodery.Web.Models
{
    /// <summary>
    /// The default response format sent to the clients.
    /// </summary>
    public class DefaultResponse
    {
        /// <summary>
        /// Indicates if the operation was successful.
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// Message that contains information about the operation.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Contains all the data, that needs to be send.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
}
