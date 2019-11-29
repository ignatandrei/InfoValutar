using InfoValutarShared;
using System;
using System.Threading.Tasks;

namespace InfovalutarDB
{
    public interface IRetrieve
    {
        public Task<ExchangeRates[]> Rates(string bank, DateTime fromDate, DateTime toDate);
        
    }
}
