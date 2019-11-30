using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using InfovalutarDB.Models;
using InfoValutarShared;
using Microsoft.EntityFrameworkCore;

namespace InfovalutarDB
{
    public class RetrieveSqlServer : IRetrieve
    {
        private readonly InMemoryDB mem;

        public RetrieveSqlServer(InMemoryDB mem)
        {
            this.mem = mem;
        }

        public async Task<bool> Exists(ExchangeRates ex)
        {
            return (await Rate(ex.Bank, ex.Date, ex.ExchangeFrom)) != null;
        }
        public async Task<ExchangeRates> Rate(string bank, DateTime date, string exchangeFrom)
        {
            var opt = mem.SqlOrMemory("DBRead");
            using (var cnt = new InfoValutarContext(opt.Options))
            {
                var data = cnt.RateQ(bank).Where(it => 
                EF.Functions.DateDiffDay(it.Date , date)==0
                && it.ExchangeFrom == exchangeFrom)
                    .Select(it => new ExchangeRates()
                    {
                        Bank = bank,
                        Date = it.Date,
                        ExchangeFrom = it.ExchangeFrom,
                        ExchangeTo = it.ExchangeTo,
                        ExchangeValue = it.ExchangeValue
                    }); 
                return await data.FirstOrDefaultAsync();
            }
            
        }
        public async Task<ExchangeRates[]> Rates(string bank, DateTime fromDate, DateTime toDate)
        {
            
            var opt = mem.SqlOrMemory("DBRead");

            
            using (var cnt = new InfoValutarContext(opt.Options))
            {
                var data = cnt.RateQ(bank)
                        .Where(it => it.Date >= fromDate && it.Date <= toDate)
                            .Select(it => new ExchangeRates()
                            {
                                Bank = bank,
                                Date = it.Date,
                                ExchangeFrom = it.ExchangeFrom,
                                ExchangeTo = it.ExchangeTo,
                                ExchangeValue =it.ExchangeValue
                            });                   
                
            
                return await data.ToArrayAsync();
            }
            
        }

        public async Task<ExchangeRates[]> TodayRates(string bank)
        {
            return await Rates(bank, DateTime.Today, DateTime.Today);
        }
    }
}

