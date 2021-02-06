namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class OrderResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Order> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
