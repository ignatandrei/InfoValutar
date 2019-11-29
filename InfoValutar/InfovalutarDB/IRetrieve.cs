using InfoValutarShared;
using System;
using System.Threading.Tasks;

namespace InfovalutarDB
{
    public interface IRetrieve
    {
        public Task<ExchangeRates[]> Rates(string bank, DateTime fromDate, DateTime toDate);

        public Task<ExchangeRates> Rate(string bank, DateTime date, string exchangeFrom);

        public Task<ExchangeRates[]> TodayRates(string bank);
    }
}
