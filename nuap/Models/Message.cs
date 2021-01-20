namespace nuap.Models
{
    using Newtonsoft.Json;

    public class Message
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "hour")]
        public string Hour { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string MessageTicket { get; set; }

        [JsonProperty(PropertyName = "issues")]
        public string Issues { get; set; }

        [JsonProperty(PropertyName = "sender_id")]
        public int SenderId { get; set; }

        [JsonProperty(PropertyName = "sender_type")]
        public string SenderType { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "font_color")]
        public string FontColor { get; set; }
    }
}
