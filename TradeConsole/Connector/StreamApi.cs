using System;
using System.Threading;
using WebSocketSharp;
using Newtonsoft.Json;
using RestSharp;

namespace TradeConsole
{
    class StreamApi
    {
        public WebSocket WebSocket { get; set; }
        public string ListenKey { get; private set; }
        public StreamApi(string url, bool IsListenKey)
        {
          //  WebSocketStreamsHandler = new();
            if (IsListenKey)
            {
                ListenKey = GetListenKey();
                ListenKeyTimer();
                WebSocket = new(url + ListenKey);
            }
            else
            {
                WebSocket = new(url);
            }
            WebSocket.OnClose += WebSocket_OnClose;
        }
        public void Connect()
        {
            WebSocket.Connect();
        }
        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            WebSocket.Connect();
        }
        private static string GetListenKey()
        {
            RestClient client = new(ApiSettings.BaseUrl);
            RestRequest request = new(ApiSettings.ListenKeyUrl, Method.POST);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
            var jsonData = JsonConvert.DeserializeObject<JsonStructure.GetListenKey>(response.Content);
            return jsonData.ListenKey;
        }

        private void ListenKeyTimer()
        {
            TimerCallback tm = new(UpdateListenKey);
            Timer timer = new(tm, null, 0, 10*60*1000);
        }

        private void UpdateListenKey(object arg)
        {
            var client = new RestClient(ApiSettings.BaseUrl);
            var request = new RestRequest(ApiSettings.ListenKeyUrl, Method.PUT);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
        }
        
       

    }
}
