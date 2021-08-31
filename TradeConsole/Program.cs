using System;
using WebSocketSharp;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

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
                TimeLine = ApiSettings.TimeLine,
                MovingAverageLength = ApiSettings.MovingAverageLength,
                StochasticLenght = ApiSettings.StochasticLenght,
                StandardDeviationMult = ApiSettings.StandardDeviation

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
                    Console.WriteLine(Analysis.Spread + " " + Analysis.StandardDeviationMax + " " + Analysis.StandardDeviationMin + " " + Analysis.Stochastic);
                }
            }
        }

        public static void Processing()
        {
            if (Signal && Type != Orders.DealType && Type != DealType.NoPosition)
            {
                Tools.Push(Analysis.Spread + " " + Analysis.StandardDeviationMax + " " + Analysis.StandardDeviationMin + " " + Analysis.Stochastic + " " + Signal + " " + Type);
                Console.WriteLine(Signal + " " + Type);
                Orders.SetMakerParameters(CurrentSymbol, NextSymbol, Type);
                BinanceRequest.MakeMultyOrders(ref Orders);
                Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "order" +";"+ Orders.OrderBuy.Quantity + ";" + Orders.OrderBuy.Price + ";" + Orders.OrderSell.Quantity + ";" + Orders.OrderSell.Price);

                if (WaitOrder())
                {
                    Orders.SetTakerParameters(CurrentSymbol, NextSymbol, Type);
                    Order order;

                    if (!Orders.OrderBuy.Status)
                    {
                        order = Orders.OrderBuy;
                        BinanceRequest.MakeCancel(order);
                        BinanceRequest.MakeOrder(ref order, true);
                        Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "order" + ";" + Orders.OrderBuy.FilledQuantity + ";" + Orders.OrderBuy.Price + ";" + "" + ";" + "");

                    }
                    else if (!Orders.OrderSell.Status)
                    {
                        order = Orders.OrderSell;
                        BinanceRequest.MakeCancel(order);
                        BinanceRequest.MakeOrder(ref order, false);
                        Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "order" + ";" + "" + ";" + "" + ";" + Orders.OrderSell.Quantity + ";" + Orders.OrderSell.Price);

                    }

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
                else
                {
                    BinanceRequest.CancelOrders(ref Orders);
                }

                Orders.OrderBuy.Status = false;
                Orders.OrderSell.Status = false;
                Orders.OrderBuy.FilledQuantity = 0;
                Orders.OrderSell.FilledQuantity = 0;
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
                if(jsonData.OrderInfo.OrderId == Orders.OrderBuy.Id)
                {
                    if (jsonData.OrderInfo.OrderStatus == "PARTIALLY_FILLED")
                    {
                        Orders.OrderBuy.FilledQuantity = jsonData.OrderInfo.OrderFilledAccumulatedQuantity;
                        Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "exec" + ";" + Orders.OrderBuy.FilledQuantity + ";" + Orders.OrderBuy.Price + ";" + "" + ";" + "");

                    }

                    if (jsonData.OrderInfo.OrderStatus == "FILLED")
                    {
                        Orders.OrderBuy.Status = true;
                        Orders.OrderBuy.FilledQuantity = jsonData.OrderInfo.OrderFilledAccumulatedQuantity;
                        Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "exec" + ";" + Orders.OrderBuy.FilledQuantity + ";" + Orders.OrderBuy.Price + ";" + "" + ";" + "");

                    }

                }
                if (jsonData.OrderInfo.OrderId == Orders.OrderSell.Id)
                {
                    if (jsonData.OrderInfo.OrderStatus == "PARTIALLY_FILLED")
                    {
                        Orders.OrderSell.FilledQuantity = jsonData.OrderInfo.OrderFilledAccumulatedQuantity;
                        Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "exec" + ";" + "" + ";" + "" + ";" + Orders.OrderSell.FilledQuantity + ";" + Orders.OrderSell.Price);

                    }

                    if (jsonData.OrderInfo.OrderStatus == "FILLED")
                    {
                        Orders.OrderSell.Status = true;
                        Orders.OrderSell.FilledQuantity = jsonData.OrderInfo.OrderFilledAccumulatedQuantity;
                        Tools.CsvLog(Analysis.Spread + ";" + Analysis.StandardDeviationMax + ";" + Analysis.StandardDeviationMin + ";" + Analysis.Stochastic + ";" + NextSymbol.BestAskPrice + ";" + NextSymbol.BestBidPrice + ";" + CurrentSymbol.BestAskPrice + ";" + CurrentSymbol.BestBidPrice + ";" + "exec" + ";" + "" + ";" + "" + ";" + Orders.OrderSell.FilledQuantity + ";" + Orders.OrderSell.Price);

                    }

                }
            }
        }

        public static bool WaitOrder()
        {
            for (int i = 0; i < WaitTime; i++)
            {
                if(Orders.OrderSell.Status || Orders.OrderBuy.Status)
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            if (Orders.OrderBuy.FilledQuantity != 0 || Orders.OrderSell.FilledQuantity != 0)
            {
                return true;
            }
            return false;
        }
        public static bool WaitOrders()
        {
            for (int i = 0; i < WaitTime; i++)
            {
                if (Orders.OrderSell.Status && Orders.OrderBuy.Status)
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }
    }
}
