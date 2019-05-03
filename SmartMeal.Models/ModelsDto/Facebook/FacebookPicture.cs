using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SmartMeal.Models.ModelsDto
{
    public class FacebookPicture
    {
        public int Height { get; set; }
        public int Width { get; set; }
        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
    }
}
