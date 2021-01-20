namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ReplyTicketResponse
    {
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
