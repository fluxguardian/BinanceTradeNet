using System;
using Newtonsoft.Json;

namespace JsonStructure
{
    public class BookTicker
    {
        public string Stream { get; set; }

        public Data Data { get; set; }
    }

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

    public class O
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("c")]
        public string ClientOrderId { get; set; }

        [JsonProperty("S")]
        public string Side { get; set; }

        [JsonProperty("o")]
        public string OrderType { get; set; }

        [JsonProperty("f")]
        public string TimeInForce { get; set; }

        [JsonProperty("q")]
        public string OriginalQuantity { get; set; }

        [JsonProperty("p")]
        public string OriginalPrice { get; set; }

        [JsonProperty("ap")]
        public string AveragePrice { get; set; }

        [JsonProperty("sp")]
        public string StopPriceNW { get; set; }

        [JsonProperty("x")]
        public string ExecutionType { get; set; }

        [JsonProperty("X")]
        public string OrderStatus { get; set; }

        [JsonProperty("i")]
        public int OrderId { get; set; }

        [JsonProperty("l")]
        public string OrderLastFilledQuantity { get; set; }

        [JsonProperty("z")]
        public string OrderFilledAccumulatedQuantity { get; set; }

        [JsonProperty("L")]
        public string LastFilledPrice { get; set; }

        [JsonProperty("ma")]
        public string MarginAsset { get; set; }

        [JsonProperty("N")]
        public string CommissionAsset { get; set; }

        [JsonProperty("n")]
        public string CommissionTrade { get; set; }

        [JsonProperty("T")]
        public long OrderTradeTime { get; set; }

        [JsonProperty("t")]
        public int TradeId { get; set; }

        [JsonProperty("rp")]
        public string RealizedProfit { get; set; }

        [JsonProperty("b")]
        public string BidQuantity { get; set; }

        [JsonProperty("a")]
        public string AskQuantity  { get; set; }

        [JsonProperty("m")]
        public bool IsMaker { get; set; }

        [JsonProperty("R")]
        public bool IsReduce { get; set; }

        [JsonProperty("wt")]
        public string StopPrice { get; set; }

        [JsonProperty("ot")]
        public string OriginalOrderType { get; set; }

        [JsonProperty("ps")]
        public string PositionSide { get; set; }

        [JsonProperty("cp")]
        public bool IsCloseAll { get; set; }

        [JsonProperty("AP")]
        public string ActivationPrice { get; set; }

        [JsonProperty("cr")]
        public string CallbackRate { get; set; }

        [JsonProperty("pP")]
        public bool IsProtected { get; set; }
    }

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
        public O O { get; set; }
    }

    public class GetListenKey
    {
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; }
    }
    
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
