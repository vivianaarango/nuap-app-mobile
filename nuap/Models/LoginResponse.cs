namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class LoginResponse
    {
        [JsonProperty(PropertyName = "data")]
        public User Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
