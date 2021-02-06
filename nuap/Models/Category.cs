namespace nuap.Models
{
    using Newtonsoft.Json;

    public class Category
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }
    }
}
