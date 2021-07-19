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
        private DateTime TimeTemp = DateTime.Now;
        public StreamApi(string url, bool IsListenKey)
        {
            if (IsListenKey)
            {
                ListenKey = GetListenKey();
                Thread thread = new(ListenKeyTimer);
                thread.IsBackground = true;
                thread.Start();
                WebSocket = new(url + ListenKey);
                
            }
            else
            {
                WebSocket = new(url);
            }
            WebSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            WebSocket.OnClose += WebSocket_OnClose;
        }

        public void Connect()
        {
            WebSocket.Connect();
        }
        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Tools.Push("СОКЕТ УПАЛ, ПЫТАЕМСЯ ПОДНЯТЬ");
            WebSocket.Connect();
        }
        private static string GetListenKey()
        {
            RestClient client = new(ApiSettings.BaseUrl);
            RestRequest request = new(ApiSettings.ListenKeyUrl, Method.POST);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
            Tools.Log(response.Content);
            var jsonData = JsonConvert.DeserializeObject<Structure.GetListenKey>(response.Content);
            return jsonData.ListenKey;
        }

        private void ListenKeyTimer()
        {
            while (true)
            {
                if ((DateTime.Now - TimeTemp).Minutes >= 10)
                {
                    UpdateListenKey();
                    TimeTemp = DateTime.Now;    
                }
                Thread.Sleep(1000);
            }
        }

        private void UpdateListenKey()
        {
            var client = new RestClient(ApiSettings.BaseUrl);
            var request = new RestRequest(ApiSettings.ListenKeyUrl, Method.PUT);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
            Tools.Log("Ключ обновлен");
            Console.WriteLine("Ключ обновлен");
        }
        
       

    }
}
