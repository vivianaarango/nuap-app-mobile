namespace nuap.Models
{
    using Newtonsoft.Json;

    public class GeneralError
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
    }
}
