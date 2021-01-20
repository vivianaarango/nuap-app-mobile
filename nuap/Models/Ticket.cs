using Newtonsoft.Json;

namespace nuap.Models
{
    public class Ticket
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "user_type")]
        public string UserType { get; set; }

        [JsonProperty(PropertyName = "issues")]
        public string Issues { get; set; }

        [JsonProperty(PropertyName = "init_date")]
        public string InitDate { get; set; }

        [JsonProperty(PropertyName = "finish_date")]
        public object FinishDate { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
