using System;
using WebSocketSharp;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;


namespace TradeConsole
{
    class Program
    {
        static Structure.Data CurrentSymbol;
        static Structure.Data NextSymbol;
        static AnalysisData Analysis;
        static DBConnector Mongo;
        static Orders Orders = new();
        static bool IsAnalays = false;
        static int WaitTime = 60;
        static bool Signal;
        static DealType Type;

        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Mongo = new()
            {
                Login = ApiSettings.DBLogin,
                Password = ApiSettings.DBPassword,
                Domen = ApiSettings.DBDomen,
                DataBaseName = ApiSettings.DataBase,
                PairCollection = ApiSettings.Pair
            };
            Mongo.Connect();
            Analysis = new()
            {
                TimeLine = 0,
                MovingAverageLength = 0,
                StochasticLenght = 0,
                StandardDeviation = 0

            };
            Analysis.SetDataSpread(Mongo.GetLastValue(Analysis.TimeLine / Analysis.TimeSize * (Analysis.MovingAverageLength + Analysis.StochasticLenght)));

            StreamApi BookTicker = new(ApiSettings.BookTickerUrl, false);
            StreamApi StreamsHandler = new(ApiSettings.StreamsHandlerUrl, true);
            BookTicker.WebSocket.OnMessage += BookTicker_OnMessage;
            StreamsHandler.WebSocket.OnMessage += StreamsHandler_OnMessage;
            BookTicker.Connect();
            StreamsHandler.Connect();
            while (true)
            {
                string command = Console.ReadLine();
                if(command == "")
                {
                    Console.WriteLine(Analysis.Spread + " " + Analysis.MovingAverage * (1 + Analysis.StandardDeviation) + " " + Analysis.MovingAverage * (1 - Analysis.StandardDeviation) + " " + Analysis.Stochastic);
                }

            }

        }
        

        public static void Processing()
        {
            if (Signal && Type != Orders.DealType && Type != DealType.NoPosition)
            {
                Tools.Push(Analysis.Spread + " " + Analysis.MovingAverage * (1 + Analysis.StandardDeviation) + " " + Analysis.MovingAverage * (1 - Analysis.StandardDeviation) + " " + Analysis.Stochastic);
                Tools.Push(Signal + " " + Type);
                Console.WriteLine(Signal + " " + Type);
                Orders.SetMarketParameters(CurrentSymbol, NextSymbol, Type);
                BinanceRequest.MakeMultyOrders(ref Orders);
                if (WaitOrders())
                {
                    if (Orders.Position)
                    {
                        Orders.DealType = DealType.NoPosition;
                    }
                    else
                    {
                        Orders.DealType = Type;
                    }

                    Orders.Position = !Orders.Position;
                }
                else
                {
                    BinanceRequest.CancelOrders(ref Orders);
                }
            }
        }

        private static void BookTicker_OnMessage(object sender, MessageEventArgs e)
        {
            var jsonData = JsonConvert.DeserializeObject<Structure.BookTicker>(e.Data);
            if (jsonData != null)
            {
                if (jsonData.Data.Symbol == ApiSettings.CurrentSymbol)
                {
                    CurrentSymbol = jsonData.Data;
                }
                if (jsonData.Data.Symbol == ApiSettings.NextSymbol)
                {
                    NextSymbol = jsonData.Data;
                }
                if (CurrentSymbol != null && NextSymbol != null && !IsAnalays)
                {
                    IsAnalays = true;
                    Analysis.SetSpread(CurrentSymbol, NextSymbol);
                    Analysis.Analaysis();
                    Signal = Analysis.Signal;
                    Type = Analysis.DealType;
                    Processing();
                    IsAnalays = false;
                }
                
            }

        }

        private static void StreamsHandler_OnMessage(object sender, MessageEventArgs e)
        {
            var jsonData = JsonConvert.DeserializeObject<Structure.StreamsHandler>(e.Data);
            if (jsonData.OrderInfo != null)
            {
                if(jsonData.OrderInfo.OrderId == Orders.OrderBuy.Id && jsonData.OrderInfo.OrderStatus == "FILLED")
                {
                    Orders.OrderBuy.Status = true;
                }
                if (jsonData.OrderInfo.OrderId == Orders.OrderSell.Id && jsonData.OrderInfo.OrderStatus == "FILLED")
                {
                    Orders.OrderSell.Status = true;
                }
            }
        }

        public static bool WaitOrders()
        {
            for (int i = 0; i < WaitTime; i++)
            {
                if(Orders.OrderSell.Status && Orders.OrderBuy.Status)
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }
    }
}
