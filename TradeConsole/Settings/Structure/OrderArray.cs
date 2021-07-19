using Newtonsoft.Json;

namespace Structure
{
    public class OrderArray
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("avgPrice")]
        public string AvgPrice { get; set; }

        [JsonProperty("origQty")]
        public string OrigQty { get; set; }

        [JsonProperty("executedQty")]
        public string ExecutedQty { get; set; }

        [JsonProperty("cumQty")]
        public string CumQty { get; set; }

        [JsonProperty("cumBase")]
        public string CumBase { get; set; }

        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reduceOnly")]
        public string ReduceOnly { get; set; }

        [JsonProperty("closePosition")]
        public string ClosePosition { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("positionSide")]
        public string PositionSide { get; set; }

        [JsonProperty("stopPrice")]
        public string StopPrice { get; set; }

        [JsonProperty("workingType")]
        public string WorkingType { get; set; }

        [JsonProperty("priceProtect")]
        public string PriceProtect { get; set; }

        [JsonProperty("origType")]
        public string OrigType { get; set; }

        [JsonProperty("updateTime")]
        public string UpdateTime { get; set; }
    }
}