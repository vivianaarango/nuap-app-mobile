namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class TicketResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Ticket> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
