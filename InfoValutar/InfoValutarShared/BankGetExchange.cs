using System;
using System.Collections.Generic;
using System.Text;

namespace InfoValutarShared
{
    public interface BankGetExchange
    {
        IAsyncEnumerable<ExchangeRates> GetActualRates();
    }
}
