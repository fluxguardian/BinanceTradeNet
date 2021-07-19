using Newtonsoft.Json;

namespace Structure
{
    public class StreamsHandler
    {
        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventTime { get; set; }

        [JsonProperty("T")]
        public long Transaction_Time { get; set; }

        [JsonProperty("i")]
        public string AccountAlias { get; set; }

        [JsonProperty("o")]
        public OrderInfo OrderInfo { get; set; }
    }
}
