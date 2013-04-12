using StockSharp.BusinessEntities;
using StockSharp.Quik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAvail.Trader.Quick
{
    public static class Extensions
    {
        public static Task<IEnumerable<MyTrade>> GetOrderTradesAsync(this QuikTrader QuikTrader, StockSharp.BusinessEntities.Order Order)
        {
            var tcs = new TaskCompletionSource<IEnumerable<MyTrade>>();

            List<MyTrade> res = new List<MyTrade>();

            QuikTrader.NewMyTrades += (IEnumerable<MyTrade> obj) =>
            {
                res.AddRange(obj.Where(p => p.Order.TransactionId == Order.TransactionId));

                if (obj.Sum(p => p.Trade.Volume) == Order.Volume)
                {
                    tcs.SetResult(obj);
                }
            };

            QuikTrader.OrdersRegisterFailed += (IEnumerable<OrderFail> obj) =>
            {
                tcs.SetException(new OrderTradesException(res));
            };

            return tcs.Task;
        }

    }

    public class OrderTradesException : Exception
    {
        public OrderTradesException(IEnumerable<MyTrade> CompletedTrades)
        {
            _completedTrades = CompletedTrades;
        }

        private readonly IEnumerable<MyTrade> _completedTrades;

        public IEnumerable<MyTrade> CompletedTrades { get { return _completedTrades; } }
    }
}
