using Newtonsoft.Json;

namespace Structure
{
    public class GetListenKey
    {
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; }
    }
}