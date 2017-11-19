using Newtonsoft.Json;

namespace Foodery.Web.Models.Auth.Register
{
    public class RegisterResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
    }
}
