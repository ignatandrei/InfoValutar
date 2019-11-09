using System;
using System.Collections.Generic;
using System.Text;

namespace InfoValutarShared
{
#pragma warning disable IDE1006 // Naming Styles
    public interface BankGetExchange
#pragma warning restore IDE1006 // Naming Styles
    {
        public string Bank { get; }
        IAsyncEnumerable<ExchangeRates> GetActualRates();
    }
}
