namespace nuap.Models
{
    using Newtonsoft.Json;

    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "user_type")]
        public string UserType { get; set; }

        [JsonProperty(PropertyName = "cancel_reason")]
        public object CancelReason { get; set; }

        [JsonProperty(PropertyName = "client_id")]
        public int ClientId { get; set; }

        [JsonProperty(PropertyName = "client_type")]
        public string ClientType { get; set; }

        [JsonProperty(PropertyName = "total_products")]
        public int TotalProducts { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public string TotalAmount { get; set; }

        [JsonProperty(PropertyName = "delivery_amount")]
        public string DeliveryAmount { get; set; }

        [JsonProperty(PropertyName = "total_discount")]
        public string TotalDiscount { get; set; }

        [JsonProperty(PropertyName = "total")]
        public string Total { get; set; }

        [JsonProperty(PropertyName = "delivery_date")]
        public string DeliveryDate { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "status_color")]
        public string StatusColor { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }

        [JsonProperty(PropertyName = "rating_image")]
        public string RatingImage { get; set; }
    }
}
