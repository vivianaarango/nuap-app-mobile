namespace nuap.Models
{
    using Newtonsoft.Json;

    public class User
    {
        [JsonProperty(PropertyName ="id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string role { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string phone { get; set; }

        [JsonProperty(PropertyName = "phone_validated")]
        public int phone_validated { get; set; }

        [JsonProperty(PropertyName = "phone_validated_date")]
        public object phone_validated_date { get; set; }

        [JsonProperty(PropertyName = "api_token")]
        public string api_token { get; set; }
    }
}
