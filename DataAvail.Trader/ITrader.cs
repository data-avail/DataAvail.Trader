using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAvail.Trader
{
    public interface ITrader : IDisposable
    {
        void Connect();

        Portfolio GetPortfolio(string Name);

        Trade[] GetTrades(string Portfolio);

        IEnumerable<Trade> CreateTrade(Order Trade);
    }
}
