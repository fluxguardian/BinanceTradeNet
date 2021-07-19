using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradeConsole
{
    class AnalysisData
    {
        public int MovingAverageLength { get; set; }
        public int StochasticLenght { get; set; }
        public double StandardDeviation { get; set; }
        public int TimeLine { get; set; }
        public int TimeSize { get; set; } = 5;
        public double Spread { get; set; }
        public double Stochastic { get; set; }
        public DealType DealType { get; set; } = DealType.NoPosition;
        public bool Signal { get; set; } = false;
        private List<double> SpreadList = new();
        private List<double> StochasticList = new();
        public double MovingAverage { get; set; }
        private DateTime TimeTemp = DateTime.Now;
        private DateTime TimeTempPush = DateTime.Now;

        public void SetDataSpread(List<Structure.DBData> datas)
        {
            datas.Reverse();
            for (int i = 0; i < datas.Count; i += TimeLine / TimeSize)
            {
                
                if (SpreadList.Count == MovingAverageLength)
                {
                    SetLastSpread(datas[i].Spread);
                }
                else
                { 
                    SpreadList.Add(datas[i].Spread);
                }
            }
            SetMovingAverage();
        }

        public void SetLastSpread(double spread)
        {
            SpreadList.RemoveAt(0);
            SpreadList.Add(spread);
            SetMovingAverage();
            SetStochastic();
        }

        public void SetSpread(Structure.Data currentSymbol, Structure.Data nextSymbol)
        {
            double currentMiddle = (Convert.ToDouble(currentSymbol.BestAskPrice) + Convert.ToDouble(currentSymbol.BestBidPrice)) / 2;
            double NextMiddle = (Convert.ToDouble(nextSymbol.BestAskPrice) + Convert.ToDouble(nextSymbol.BestBidPrice)) / 2;
            Spread = NextMiddle - currentMiddle;
            var timeNow = DateTime.Now;
            if((timeNow - TimeTemp).Seconds >= TimeLine)
            {
                TimeTemp = timeNow;
                SetLastSpread(Spread);
            }
        }

        private void SetMovingAverage()
        {
            double sum = 0;
            foreach(double i in SpreadList)
            {
                sum += i;
            }
            MovingAverage = sum / MovingAverageLength;
        }

        private void SetStochastic()
        {
            double max = SpreadList.Max();
            double min = SpreadList.Min();
            double st = 100 * (SpreadList[^1] - min) / (max - min);
            StochasticList.Add(st);
            if (StochasticList.Count == StochasticLenght)
            {
                StochasticMean();
                StochasticList.RemoveAt(0);
            }
        }

        private void StochasticMean()
        {
            double sum = 0;
            foreach (double i in StochasticList)
            {
                sum += i;
            }
            Stochastic = sum / StochasticLenght;
        }

        public void Analaysis()
        {
            if (Spread != 0)
            {

                if (Spread > MovingAverage * (1 + StandardDeviation) && Stochastic >80)
                {
                    Signal = true;
                    DealType = DealType.Short;
                }
                else if (Spread < MovingAverage * (1 - StandardDeviation) && Stochastic < 20)
                {
                    Signal = true;
                    DealType = DealType.Long;
                }
                else
                {
                    Signal = false;
                    DealType = DealType.NoPosition;
                }

                var timeNow = DateTime.Now;
                if ((timeNow - TimeTempPush).Minutes >= 10 && (Stochastic > 79 || Stochastic < 21))
                {
                    TimeTempPush = timeNow;

                    Tools.Push(Spread + " " + MovingAverage * (1 + StandardDeviation) + " " + MovingAverage * (1 - StandardDeviation) + " " + Stochastic);
                    Tools.Log(Spread + " " + MovingAverage * (1 + StandardDeviation) + " " + MovingAverage * (1 - StandardDeviation) + " " + Stochastic);
                }
            }
            else
            {
                Signal = false;
                DealType = DealType.NoPosition;
            }

        }

    }
}
