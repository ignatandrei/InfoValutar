using InfoValutarShared;
using System;
using System.Collections.Generic;

namespace InfoValutarPluginStarter
{
    public class GetFakeExchangeRates : BankGetExchange
    {
        public string Bank => "FakeBank";

        public async IAsyncEnumerable<ExchangeRates> GetActualRates()
        {
            yield return new ExchangeRates()
            {
                Bank = this.Bank,
                ExchangeFrom = "Fake1",
                ExchangeTo = "Fake2",
                Date = DateTime.Now,
                ExchangeValue = 1
            };
        }
    }
}
