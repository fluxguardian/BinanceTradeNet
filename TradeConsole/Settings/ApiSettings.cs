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
        public static string Pair { get; private set; } = "BTCUSD";
        public static string BaseUrl { get; private set; } = "https://dapi.binance.com/";
        public static string OrderUrl { get; private set; } = "dapi/v1/order";
        public static string TimeUrl { get; private set; } = "dapi/v1/time";
        public static string BookUrl { get; private set; } = "dapi/v1/ticker/bookTicker?pair=" + Pair;
        public static string ListenKeyUrl { get; private set; } = "dapi/v1/listenKey";
        public static string CurrentSymbol { get; set; } = Pair.ToLower() + "_210924";
        public static string NextSymbol { get; set; } = Pair.ToLower() + "_211231";
        public static string BookTickerUrl { get; set; } = "wss://dstream.binance.com/stream?streams=" + CurrentSymbol + "@bookTicker/" + NextSymbol + "@bookTicker";
        public static string StreamsHandlerUrl { get; set; } = "wss://dstream.binance.com/ws/";
        public static string DBDomen { get; set; } = ConfigurationManager.AppSettings["DBDomen"];
        public static string DataBase { get; set; } = ConfigurationManager.AppSettings["DataBase"];
        public static string DBLogin { get; set; } = ConfigurationManager.AppSettings["DBLogin"];
        public static string DBPassword { get; set; } = ConfigurationManager.AppSettings["DBPassword"];
    }
}
