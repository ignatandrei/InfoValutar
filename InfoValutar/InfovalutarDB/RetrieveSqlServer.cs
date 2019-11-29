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
        public async Task<ExchangeRates[]> Rates(string bank, DateTime fromDate, DateTime toDate)
        {


            
            DbContextOptionsBuilder<InfoValutarContext> opt;
            var ConnectionString = mem.GetConRead("DBRead");

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                opt = mem.MemoryOptions();
            }
            else
            {
                opt = new DbContextOptionsBuilder<InfoValutarContext>();
                opt.UseSqlServer(ConnectionString);
            }
            IQueryable<ExchangeRates> data;
            using (var cnt = new InfoValutarContext(opt.Options))
            {

                switch (bank?.ToLower())
                {
                    case "ecb":
                        data = cnt.Ecb.Where(it => it.Date >= fromDate && it.Date <= toDate)
                            .Select(it => new ExchangeRates()
                            {
                                Bank = bank,
                                Date = it.Date,
                                ExchangeFrom = it.ExchangeFrom,
                                ExchangeTo = it.ExchangeTo
                            });
                        break;
                    case "bnr":
                        data = cnt.Nbr.Where(it => it.Date >= fromDate && it.Date < toDate)
                            .Select(it => new ExchangeRates()
                            {
                                Bank = bank,
                                Date = it.Date,
                                ExchangeFrom = it.ExchangeFrom,
                                ExchangeTo = it.ExchangeTo
                            });
                        break;
                    default:
                        throw new ArgumentException($"cannot find bank {bank}");
                }
                return await data.ToArrayAsync();
            }
            
        }
    }
}
