using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeConsole
{
    enum DealType
    {
        NoPosition = 'N',
        Short = 'S',
        Long = 'L'
    }
    class Order
    {
        public double Price { get; set; }
        public string Symbol { get; set; }
        public string Id { get; set; }
        public bool Status { get; set; }
        public int Quantity { get; set; } = 1;
        public int FilledQuantity { get; set; } = 0;
    }
    class Orders
    {
        public DealType DealType { get; set; } = DealType.NoPosition;
        public bool Position { get; set; } = false;
        public Order OrderBuy { get; set; } = new();
        public Order OrderSell { get; set; } = new();
        public int Quantity { get; set; } = ApiSettings.Quantity;
        public double AssumptionTaker { get; set; } = 0.0025;
        public double AssumptionMaker { get; set; } = 0.00005;
        private int Round = 1;

        public void SetTakerParameters(Structure.Data currentSymbol, Structure.Data nextSymbol, DealType type)
        {
            if (type == DealType.Long)
            {
                OrderBuy.Price = Math.Round(Convert.ToDouble(nextSymbol.BestAskPrice) + AssumptionTaker * Convert.ToDouble(nextSymbol.BestAskPrice), Round);
                OrderBuy.Symbol = nextSymbol.Symbol;

                OrderSell.Price = Math.Round(Convert.ToDouble(currentSymbol.BestBidPrice) - AssumptionTaker * Convert.ToDouble(currentSymbol.BestBidPrice), Round);
                OrderSell.Symbol = currentSymbol.Symbol;
            }
            else if (type == DealType.Short)
            {
                OrderSell.Price = Math.Round(Convert.ToDouble(nextSymbol.BestBidPrice) - AssumptionTaker * Convert.ToDouble(nextSymbol.BestAskPrice), Round);
                OrderSell.Symbol = nextSymbol.Symbol;

                OrderBuy.Price = Math.Round(Convert.ToDouble(currentSymbol.BestAskPrice) + AssumptionTaker * Convert.ToDouble(currentSymbol.BestBidPrice), Round);
                OrderBuy.Symbol = currentSymbol.Symbol;
            }
            OrderBuy.Quantity = Math.Abs(OrderSell.FilledQuantity - OrderBuy.FilledQuantity);
            OrderSell.Quantity = Math.Abs(OrderBuy.FilledQuantity - OrderSell.FilledQuantity);
        }
        public void SetMakerParameters(Structure.Data currentSymbol, Structure.Data nextSymbol, DealType type)
        {
            if (type == DealType.Long)
            {
                OrderBuy.Price = Math.Round(Convert.ToDouble(nextSymbol.BestBidPrice) - AssumptionMaker * Convert.ToDouble(nextSymbol.BestBidPrice), Round);
                OrderBuy.Symbol = nextSymbol.Symbol;

                OrderSell.Price = Math.Round(Convert.ToDouble(currentSymbol.BestAskPrice) + AssumptionMaker * Convert.ToDouble(currentSymbol.BestAskPrice), Round);
                OrderSell.Symbol = currentSymbol.Symbol;
            }
            else if (type == DealType.Short)
            {
                OrderSell.Price = Math.Round(Convert.ToDouble(nextSymbol.BestAskPrice) + AssumptionMaker * Convert.ToDouble(nextSymbol.BestBidPrice), Round);
                OrderSell.Symbol = nextSymbol.Symbol;

                OrderBuy.Price = Math.Round(Convert.ToDouble(currentSymbol.BestBidPrice) - AssumptionMaker * Convert.ToDouble(currentSymbol.BestAskPrice), Round);
                OrderBuy.Symbol = currentSymbol.Symbol;
            }
            OrderBuy.Quantity = Quantity;
            OrderSell.Quantity = Quantity;
        }
    }
}
