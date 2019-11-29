using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfovalutarDB.Models;
using InfoValutarShared;
using Microsoft.EntityFrameworkCore;

namespace InfovalutarDB
{
    public class SaveSqlServer : ISave
    {
        public SaveSqlServer()
        {

        }
        public async Task<int> Save(params ExchangeRates[] er)
        {
            var data = er.Select(it => it.Bank?.ToLower()).Distinct();

            DbContextOptionsBuilder<InfoValutarContext> opt;
            var ConnectionString = InMemoryDB.sing.GetConRead("DBRead");

            if (string.IsNullOrWhiteSpace(ConnectionString ))
            {
                opt = InMemoryDB.sing.MemoryOptions();
            }
            else
            {
                opt = new DbContextOptionsBuilder<InfoValutarContext>();
                opt.UseSqlServer(ConnectionString);
            }

            using (var cnt = new InfoValutarContext(opt.Options))
            {
                foreach (var item in data)
                {
                    var dataToSave = er
                        .Where(it => it.Bank?.ToLower() == item);


                    switch (item)
                    {
                        case "ecb":
                            cnt.Ecb.AddRange(
                                dataToSave.Select(it =>
                                new Ecb()
                                {
                                    Date = it.Date,
                                    ExchangeFrom = it.ExchangeFrom,
                                    ExchangeTo = it.ExchangeTo,
                                    ExchangeValue = it.ExchangeValue
                                }).ToArray());
                            break;
                        case "bnr":
                            cnt.Nbr.AddRange(
                                dataToSave.Select(it =>
                                new Nbr()
                                {
                                    Date = it.Date,
                                    ExchangeFrom = it.ExchangeFrom,
                                    ExchangeTo = it.ExchangeTo,
                                    ExchangeValue = it.ExchangeValue
                                }).ToArray());
                            break;
                        default:
                            throw new ArgumentException($"cannot save bank {item}");
                    }

                }
                var nr= await cnt.SaveChangesAsync();               
                return nr;
            }

        }
    }
}
