namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CategoryResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Category> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
