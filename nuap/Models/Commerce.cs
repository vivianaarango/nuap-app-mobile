namespace nuap.Models
{
    using Newtonsoft.Json;

    public class Commerce
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "business_name")]
        public string BusinessName { get; set; }

        [JsonProperty(PropertyName = "nit")]
        public string Nit { get; set; }

        [JsonProperty(PropertyName = "commission")]
        public string Commission { get; set; }

        [JsonProperty(PropertyName = "url_logo")]
        public string UrlLogo { get; set; }

        [JsonProperty(PropertyName = "shipping_cost")]
        public string ShippingCost { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public string Distance { get; set; }
    }
}
