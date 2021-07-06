using System;
using WebSocketSharp;
using System.Configuration;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TradeConsole
{
    class Program
    {
        static void Main()
        {
            DBConnector mongo = new()
            {
                Login = ApiSettings.DBLogin,
                Password = ApiSettings.DBPassword,
                Domen = ApiSettings.DBDomen,
                DataBaseName = ApiSettings.DataBase,
                PairCollection = ApiSettings.Pair
            };
            mongo.Connect();
            AnalysisData analysis = new()
            {
                TimeLine = 20,
                MovingAverageLength = 1800,
                StandardDeviation= 0.13
                
            };
            StreamApi BookTicker = new(ApiSettings.BookTickerUrl, false);
            StreamApi StreamsHandler = new(ApiSettings.StreamsHandlerUrl, true);
            BookTicker.WebSocket.OnMessage += BookTicker_OnMessage;
            BookTicker.WebSocket.OnMessage += StreamsHandler_OnMessage;
            BookTicker.Connect();
            StreamsHandler.Connect();


        }

        private static void BookTicker_OnMessage(object sender, MessageEventArgs e)
        {
            var jsonData = JsonConvert.DeserializeObject<JsonStructure.BookTicker>(e.Data);
            

        }
        private static void StreamsHandler_OnMessage(object sender, MessageEventArgs e)
        {
            var jsonData = JsonConvert.DeserializeObject<JsonStructure.StreamsHandler>(e.Data);

        }

        private static void AddLastSpred(DBConnector mongo, AnalysisData analysis)
        {
            var lastSpread = mongo.GetLastValue();
            if (analysis.IsNewSpred(lastSpread))
            {
                analysis.SetLastSpread(lastSpread);
            }
        }
    }
}
