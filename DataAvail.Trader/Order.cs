using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAvail.Trader
{
    public class Order
    {
        public string Symbol { get; set; }

        public string Portfolio { get; set; }

        public string SubPortfolio { get; set; }

        public int Volume { get; set; }
    }
}
