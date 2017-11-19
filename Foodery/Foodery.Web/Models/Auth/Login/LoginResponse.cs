using Newtonsoft.Json;

namespace Foodery.Web.Models.Auth.Login
{
    public class LoginResponse
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
