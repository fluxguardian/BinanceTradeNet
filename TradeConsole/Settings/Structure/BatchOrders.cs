using Newtonsoft.Json;
using System.Collections.Generic;

namespace Structure
{
    public class BatchOrders
    {
        [JsonProperty("OrderArray")]
        public List<OrderArray> OrderArray { get; set; }
    }
}