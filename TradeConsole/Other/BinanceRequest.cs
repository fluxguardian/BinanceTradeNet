using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TradeConsole
{
    class BinanceRequest
    {
        public static void MakeMultyOrders(ref Orders orders)
        {
            string arg = $"batchOrders=%5B%7B%22symbol%22%3A+%22{orders.OrderBuy.Symbol}%22%2C+%22side%22%3A+%22BUY%22%2C+%22type%22%3A+%22LIMIT%22%2C+%22quantity%22%3A+%22{orders.Quantity}%22%2C+%22price%22%3A+%22{(int)orders.OrderBuy.Price}%22%2C+%22timeInForce%22%3A+%22GTC%22%7D%2C+%7B%22symbol%22%3A+%22{orders.OrderSell.Symbol}%22%2C+%22side%22%3A+%22SELL%22%2C+%22type%22%3A+%22LIMIT%22%2C+%22quantity%22%3A+%22{orders.Quantity}%22%2C+%22price%22%3A+%22{(int)orders.OrderSell.Price}%22%2C+%22timeInForce%22%3A+%22GTC%22%7D%5D&timestamp=" + DateTimeOffset.Now.ToUnixTimeMilliseconds();
            RestClient client = new(ApiSettings.BaseUrl);
            RestRequest request = new(ApiSettings.BatchOrdersUrl + "?" + arg + "&signature=" + Tools.HashSHA256(Encoding.UTF8.GetBytes(arg), Encoding.UTF8.GetBytes(ApiSettings.SecretKey)), Method.POST);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Tools.Log(response.Content);
            var jsonData = JsonConvert.DeserializeObject<Structure.BatchOrders>("{\"OrderArray\":"+response.Content+"}");
            orders.OrderBuy.Id = jsonData.OrderArray[0].OrderId;
            orders.OrderSell.Id = jsonData.OrderArray[1].OrderId;
        }
        
        public static void CancelOrders(ref Orders orders)
        {
            if (!orders.OrderBuy.Status)
            {
                MakeCancel(orders.OrderBuy);
            }
            else
            {
                ReMakeOrder(orders.OrderBuy, true, orders.Quantity);
            }

            if (!orders.OrderSell.Status)
            {
                MakeCancel(orders.OrderSell);
            }
            else
            {
                ReMakeOrder(orders.OrderSell, true, orders.Quantity);
            }
            orders.OrderBuy.Status = false;
            orders.OrderSell.Status = false;
            orders.DealType = DealType.NoPosition;
        }

        private static void MakeCancel(Order order)
        {
            string arg = $"symbol={order.Symbol}&orderId={order.Id}&timestamp=" + DateTimeOffset.Now.ToUnixTimeMilliseconds()+500;
            RestClient client = new(ApiSettings.BaseUrl);
            RestRequest request = new(ApiSettings.BatchOrdersUrl + "?" + arg + "&signature=" + Tools.HashSHA256(Encoding.UTF8.GetBytes(arg), Encoding.UTF8.GetBytes(ApiSettings.SecretKey)), Method.POST);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Tools.Log(response.Content);
        }

        private static void ReMakeOrder(Order order, bool isBuy, int quantity)
        {
            string side;
            if (isBuy)
            {
                side = "SELL";
            }
            else
            {
                side = "BUY";
            }

            string arg = $"symbol={order.Symbol}&side={side}&type=MARKET&quantity={quantity}&timestamp=" + DateTimeOffset.Now.ToUnixTimeMilliseconds();
            RestClient client = new(ApiSettings.BaseUrl);
            RestRequest request = new(ApiSettings.BatchOrdersUrl + "?" + arg + "&signature=" + Tools.HashSHA256(Encoding.UTF8.GetBytes(arg), Encoding.UTF8.GetBytes(ApiSettings.SecretKey)), Method.POST);
            request.AddHeader("X-MBX-APIKEY", ApiSettings.ApiKey);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            Tools.Log(response.Content);
        }
    }
}
