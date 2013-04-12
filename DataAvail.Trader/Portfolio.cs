using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAvail.Trader
{
    public class Portfolio
    {
        internal Portfolio(string Name, decimal BeginValue, decimal CurrentValue)
        {
            _name = Name;

            _beginValue = BeginValue;

            _currentValue = CurrentValue;
        }

        private readonly string _name;

        private readonly decimal _beginValue;

        private readonly decimal _currentValue;

        public string Name { get { return _name; } }

        public decimal BeginValue { get { return _beginValue; } }

        public decimal CurrentValue { get { return _currentValue; } }
    }
}
