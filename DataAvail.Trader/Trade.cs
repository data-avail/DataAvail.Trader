using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAvail.Trader
{
    public class Trade : Order
    {
        internal Trade(decimal Price, System.DateTime Date)
        {
            this._price = Price;

            this._date = Date;
        }

        private readonly decimal _price;

        private readonly System.DateTime _date;

        public decimal Price { get { return _price; } }

        public System.DateTime Date { get { return _date; } }
    }
}
