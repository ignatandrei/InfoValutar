using System;
using System.Collections.Generic;
using System.Text;

namespace InfoValutarShared
{
    public class ExchangeRates
    {
        public string Bank { get; set; }
        public DateTime Date { get; set; }
        public string ExchangeFrom { get; set; }
        public string ExchangeTo { get; set; }
        public decimal ExchangeValue { get; set; }
    }
}
