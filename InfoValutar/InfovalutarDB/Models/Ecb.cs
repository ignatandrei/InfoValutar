using System;
using System.Collections.Generic;

namespace InfovalutarDB.Models
{
    public partial class Ecb
    {
        public string ExchangeFrom { get; set; }
        public string ExchangeTo { get; set; }
        public DateTime Date { get; set; }
        public decimal ExchangeValue { get; set; }
    }
}
