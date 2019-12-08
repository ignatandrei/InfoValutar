using InfoValutarShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoValutarPluginStarter
{
    public class GetFakeExchangeRates : BankGetExchange
    {
        public string Bank => "FakeBank";

        public async Task<IEnumerable<ExchangeRates>> GetActualRates()
        {
            await Task.Delay(10);
            return new[] {
            new ExchangeRates()
            {
                Bank = this.Bank,
                ExchangeFrom = "Fake1",
                ExchangeTo = "Fake2",
                Date = DateTime.Now,
                ExchangeValue = 1
            }
            };
        }
    }
}
