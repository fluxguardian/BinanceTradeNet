using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TradeConsole
{
    class Tools
    {
        public static void Push(string body)
        {
            var client = new RestClient(ApiSettings.PushBaseUrl);
            var request = new RestRequest(ApiSettings.PushUrl, Method.POST);
            request.AddParameter("type", "note");
            request.AddParameter("title", "BinanceBot");
            request.AddParameter("body", DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss") + ": " + body);
            request.AddHeader("Access-Token", ApiSettings.ApiPush);
            var response = client.Execute(request);
            Log(response.Content);
        }

        public static void Log(string message)
        {
            File.AppendAllText("log.txt", DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss") + ": " + message + "\n");
        }

        public static string HashSHA256(byte[] data, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            var a = hmac.ComputeHash(data);
            string res = "";
            foreach (byte b in a)
            {
                res += b.ToString("x2");
            }
            return res;
        }
    }
}
