using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeConsole
{
    class AnalysisData
    {
        public int MovingAverageLength { get; set; }
        public double StandardDeviation { get; set; }
        public int TimeLine { get; set; }
        private int TimeSize { get; set; } = 5;
        private List<double> SpreadList { get; set; }
        private string LastSpreadId;
        private int TempTime = 0;


        public void SetDataSpread(List<JsonStructure.DBData> datas)
        {
            for(int i = 0; i <= datas.Count; i += TimeLine / TimeSize)
            {
                SpreadList.Add(datas[i].Spread);
                LastSpreadId = datas[i].Id;
            }
        }

        public void SetLastSpread(List<JsonStructure.DBData> datas)
        {
            SpreadList.RemoveAt(0);
            SpreadList.Add(datas[0].Spread);
        }

        public bool IsNewSpred(List<JsonStructure.DBData> data)
        {
            if (data[0].Id != LastSpreadId)
            {
                LastSpreadId = data[0].Id;
                TempTime++;
                if(TempTime == TimeLine / TimeSize)
                {
                    TempTime = 0;
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

    }
}
