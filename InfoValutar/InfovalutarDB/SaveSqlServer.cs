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
            var data = er.Select(it => it.Bank.ToLower()).Distinct();
            
            var config = ConfigurationManager.ConnectionStrings["DB"];
            var opt= new DbContextOptionsBuilder<InfoValutarContext>();
            if (config == null)
            {
                opt.UseInMemoryDatabase("write");
            }
            else
            {
                opt.UseSqlServer(config.ConnectionString);
            }

            var cnt = new InfoValutarContext(opt.Options);
            foreach (var item in data)
            {
                var dataToSave = er
                    .Where(it => it.Bank.ToLower() == item);
                    

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
            return await cnt.SaveChangesAsync();

        }
    }
}
