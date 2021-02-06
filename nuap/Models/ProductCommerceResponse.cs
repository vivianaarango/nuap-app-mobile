namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ProductResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Product> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
