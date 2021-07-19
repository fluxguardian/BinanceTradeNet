using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace TradeConsole
{
    class DBConnector
    {
        public string Domen { get; set; }
        public string Port { get; set; }
        public string DataBaseName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PairCollection { get; set; }
        private IMongoDatabase DataBase { get; set; }
        private IMongoCollection<BsonDocument> Collection { get; set; }


        public void Connect()
        {
            string db = $"mongodb://{Login}:{Password}@{Domen}/{DataBaseName}";
            MongoClient client = new(db);
            DataBase = client.GetDatabase(DataBaseName);
            Collection = DataBase.GetCollection<BsonDocument>(PairCollection);
        }

        public List<Structure.DBData> GetLastValue(int count)
        {
            return LastValues(count);
        }

        public List<Structure.DBData> GetLastValue()
        {
            return LastValues(1);
        }

        private List<Structure.DBData> LastValues(int count)
        {
            var findedList = Collection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(count).ToList();
            var findedArray = new List<Structure.DBData>();
            foreach (var line in findedList)
            {
                string jsonLine = line.ToString().Replace("NumberLong(", "").Replace(")", "");
                findedArray.Add(JsonConvert.DeserializeObject<Structure.DBData>(jsonLine));
            }
            return findedArray;
        }
    }
}
