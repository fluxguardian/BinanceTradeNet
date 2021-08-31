using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace TradeConsole
{
    static class ApiSettings
    {
        public static string SecretKey { get; private set; } = ConfigurationManager.AppSettings["SecretKey"];
        public static string ApiKey { get; private set; } = ConfigurationManager.AppSettings["ApiKey"];
        public static string ApiPush { get; private set; } = ConfigurationManager.AppSettings["PushKey"];
        public static string Pair { get; private set; } = "BNBUSD";
        public static string BaseUrl { get; private set; } = "https://ftx.com/api";
        public static string PushBaseUrl { get; private set; } = "https://api.pushbullet.com/";
        public static string PushUrl { get; private set; } = "v2/pushes";
        public static string OrderUrl { get; private set; } = "dapi/v1/order";
        public static string TimeUrl { get; private set; } = "dapi/v1/time";
        public static string BookUrl { get; private set; } = "dapi/v1/ticker/bookTicker?pair=" + Pair;
        public static string ListenKeyUrl { get; private set; } = "dapi/v1/listenKey";
        public static string BatchOrdersUrl { get; private set; } = "/dapi/v1/batchOrders";
        public static string CurrentSymbol { get; set; } = Pair + "_210924";
        public static string NextSymbol { get; set; } = Pair + "_211231";
        public static int Quantity { get; set; } = int.Parse(ConfigurationManager.AppSettings["Quantity"]);
        public static int TimeLine { get; private set; } = int.Parse(ConfigurationManager.AppSettings["TimeLine"]);
        public static int MovingAverageLength { get; private set; } = int.Parse(ConfigurationManager.AppSettings["MovingAverageLength"]);
        public static int StochasticLenght { get; private set; } = int.Parse(ConfigurationManager.AppSettings["StochasticLenght"]);
        public static double StandardDeviation { get; private set; } = double.Parse(ConfigurationManager.AppSettings["StandardDeviation"]);

        public static string BookTickerUrl { get; set; } = "wss://dstream.binance.com/stream?streams=" + CurrentSymbol.ToLower() + "@bookTicker/" + NextSymbol.ToLower() + "@bookTicker";
        public static string StreamsHandlerUrl { get; set; } = "wss://dstream.binance.com/ws/";
        public static string DBDomen { get; set; } = ConfigurationManager.AppSettings["DBDomen"];
        public static string DataBase { get; set; } = ConfigurationManager.AppSettings["DataBase"];
        public static string DBLogin { get; set; } = ConfigurationManager.AppSettings["DBLogin"];
        public static string DBPassword { get; set; } = ConfigurationManager.AppSettings["DBPassword"];
    }
}
