namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ProfileCommerceResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Commerce Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
