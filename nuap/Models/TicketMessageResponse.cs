namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class TicketMessageResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Message> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
