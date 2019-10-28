using System;
using System.Collections.Generic;
using System.Text;

namespace InfoValutarDOS
{
    class ExchangeRates
    {
        public DateTime date { get; set; }
        public string ExchangeFrom { get; set; }
        public string ExchangeTo { get; set; }
        public decimal ExchangeValue { get; set; }
    }
}
