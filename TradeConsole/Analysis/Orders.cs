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
    }
    class Orders
    {
        public DealType DealType { get; set; } = DealType.NoPosition;
        public bool Position { get; set; } = false;
        public Order OrderBuy { get; set; } = new();
        public Order OrderSell { get; set; } = new();
        public int Quantity { get; set; } = 1;
        public int Assumption { get; set; } = 100;

        public void SetMarketParameters(Structure.Data currentSymbol, Structure.Data nextSymbol, DealType type)
        {
            if (type == DealType.Long)
            {
                OrderBuy.Price = Convert.ToDouble(nextSymbol.BestAskPrice) + Assumption;
                OrderBuy.Symbol = nextSymbol.Symbol;

                OrderSell.Price = Convert.ToDouble(currentSymbol.BestBidPrice) - Assumption;
                OrderSell.Symbol = currentSymbol.Symbol;
            }
            else if (type == DealType.Short)
            {
                OrderSell.Price = Convert.ToDouble(nextSymbol.BestAskPrice) - Assumption;
                OrderSell.Symbol = nextSymbol.Symbol;

                OrderBuy.Price = Convert.ToDouble(currentSymbol.BestBidPrice) + Assumption;
                OrderBuy.Symbol = currentSymbol.Symbol;
            }
        }
    }
}
