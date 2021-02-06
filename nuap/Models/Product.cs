namespace nuap.Models
{
    using Newtonsoft.Json;

    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "is_featured")]
        public int IsFeatured { get; set; }

        [JsonProperty(PropertyName = "stock")]
        public int Stock { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "purchase_price")]
        public int PurchasePrice { get; set; }

        [JsonProperty(PropertyName = "sale_price")]
        public int SalePrice { get; set; }

        [JsonProperty(PropertyName = "special_price")]
        public int SpecialPrice { get; set; }

        [JsonProperty(PropertyName = "has_special_price")]
        public int HasSpecialPrice { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int Position { get; set; }

        [JsonProperty(PropertyName = "status_text")]
        public string StatusText { get; set; }

        [JsonProperty(PropertyName = "status_color")]
        public string StatusColor { get; set; }
    }
}
