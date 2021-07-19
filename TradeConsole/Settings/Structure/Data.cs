using Newtonsoft.Json;

namespace Structure
{
    public class Data
    {
        [JsonProperty("u")]
        public string UpdateId { get; set; }

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("ps")]
        public string Pair { get; set; }

        [JsonProperty("b")]
        public string BestBidPrice { get; set; }

        [JsonProperty("B")]
        public string BestBidQty { get; set; }

        [JsonProperty("a")]
        public string BestAskPrice { get; set; }

        [JsonProperty("A")]
        public string BestAskQty { get; set; }

        [JsonProperty("T")]
        public string TransactionTime { get; set; }

        [JsonProperty("E")]
        public string EventTime { get; set; }
    }
}
