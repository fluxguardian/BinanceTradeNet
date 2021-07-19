using Newtonsoft.Json;

namespace Structure
{
    public class DBData
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("spread")]
        public double Spread { get; set; }

        [JsonProperty("Current_bidPrice")]
        public double CurrentBidPrice { get; set; }

        [JsonProperty("Current_askPrice")]
        public double CurrentAskPrice { get; set; }

        [JsonProperty("Next_bidPrice")]
        public double NextBidPrice { get; set; }

        [JsonProperty("Next_askPrice")]
        public double NextAskPrice { get; set; }
    }
}