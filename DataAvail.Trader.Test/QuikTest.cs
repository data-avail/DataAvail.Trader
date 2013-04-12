using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataAvail.Trader.Test
{
    [TestClass]
    public class QuikTest
    {
        public static string PORTF = "000000157514";

        [TestMethod]
        public void buy_sell_1_GAZP()
        {
            using (var trader = new DataAvail.Trader.Quick.Trader())
            {
                trader.Connect();

                //buy

                var trades = trader.CreateTrade(new Order { Portfolio = PORTF, Symbol = "GAZP", Volume = 1 });

                Assert.AreEqual(1, trades.Count());

                var trade = trades.Single();
                Assert.AreEqual(1, trade.Volume);
                Assert.AreEqual(PORTF, trade.Portfolio);

                var statTrades = trader.GetTrades(PORTF).Where(p => p.Date == trade.Date);

                Assert.AreEqual(1, statTrades.Count());
                
                Assert.AreEqual(1, trade.Volume);
                Assert.AreEqual(PORTF, trade.Portfolio);

                //sell

                trades = trader.CreateTrade(new Order { Portfolio = PORTF, Symbol = "GAZP", Volume = -1 });

                Assert.AreEqual(1, trades.Count());

                trade = trades.Single();
                Assert.AreEqual(-1, trade.Volume);
                Assert.AreEqual(PORTF, trade.Portfolio);

                statTrades = trader.GetTrades(PORTF).Where(p => p.Date == trade.Date);

                Assert.AreEqual(1, statTrades.Count());

                Assert.AreEqual(-1, trade.Volume);
                Assert.AreEqual(PORTF, trade.Portfolio);
            }
        }
    }
}
