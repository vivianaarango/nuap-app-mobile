namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ProfileUserResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Client Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
