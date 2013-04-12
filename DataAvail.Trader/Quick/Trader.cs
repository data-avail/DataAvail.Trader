using StockSharp.BusinessEntities;
using StockSharp.Quik;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAvail.Trader.Quick
{
    public class Trader : ITrader
    {
        private QuikTrader _trader;

        public void Connect()
        {
            _trader = new QuikTrader(ConfigurationManager.AppSettings["QUIK_PATH"]);

            if (!_trader.IsConnected)
            {
                var task_export = Task.Factory.StartNew(() => _trader.StartExport());

                _trader.IsAsyncMode = true;

                _trader.Connect();

                task_export.Wait();
            }

        }

        public void Dispose()
        {
            if (_trader != null)
            {
                _trader.StopExport();
                _trader.Disconnect();
            }
        }

        public Trade[] GetTrades(string Portfolio)
        {
            return _trader.MyTrades.Select(p => ConvertTrade(p)).ToArray();
        }

        public DataAvail.Trader.Portfolio GetPortfolio(string Name)
        {
            var portf = _trader.Portfolios.FirstOrDefault(p => p.Name == Name);

            return portf == null ? null : new DataAvail.Trader.Portfolio(portf.Name, portf.BeginValue, portf.CurrentValue);
        }

        public IEnumerable<Trade> CreateTrade(Order Trade)
        {
            var order = new StockSharp.BusinessEntities.Order
            {
                Portfolio = _trader.Portfolios.FirstOrDefault(p => p.Name == Trade.Portfolio),
                Price = 0,
                Security = _trader.Securities.FirstOrDefault(p => p.Code == Trade.Symbol && p.Type == SecurityTypes.Stock),
                Volume = Math.Abs(Trade.Volume),
                Type = OrderTypes.Market,
                Direction = Trade.Volume > 0 ? OrderDirections.Buy : OrderDirections.Sell,
            };

            using (var task = _trader.GetOrderTradesAsync(order))
            {
                _trader.RegisterOrder(order);

                task.Wait();

                if (task.IsCanceled)
                {
                    throw new Exception("Order not executed!");
                }
                else if (task.IsFaulted)
                {
                    throw task.Exception;
                }
                else
                {
                    return task.Result.Select(p => ConvertTrade(p)).ToArray();
                }
            }
        }

        private static Trade ConvertTrade(StockSharp.BusinessEntities.MyTrade Trade)
        {
            return new Trade(Trade.Trade.Price, Trade.Trade.Time)
            {
                Volume = (int)Trade.Order.Volume * (Trade.Order.Direction == OrderDirections.Buy ? 1 : -1),
                Symbol = Trade.Trade.Security.Code,
                Portfolio = Trade.Order.Portfolio.Name
            };
        }
    }
}
