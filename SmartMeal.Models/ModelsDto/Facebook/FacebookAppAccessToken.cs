using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SmartMeal.Models.ModelsDto
{
    public class FacebookAppAccessToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
