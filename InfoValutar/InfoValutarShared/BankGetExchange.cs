using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace InfoValutarShared
{
#pragma warning disable IDE1006 // Naming Styles
    public interface BankGetExchange
#pragma warning restore IDE1006 // Naming Styles
    {
        public string Bank { get; }
        Task<IEnumerable<ExchangeRates>> GetActualRates();

        IEnumerable<ExchangeRates> TodayFromCache
        {
            get

            {
                string key = $"{this.Bank}_{DateTime.UtcNow.ToString("yyyyMMdd")}";
                var mc = MemoryCache.Default;
                if (mc.Contains(key))
                    return mc[key] as ExchangeRates[];

                return null;

            }
            set
            {
                var now = DateTime.UtcNow;
                var nextDay = now.Date.AddDays(1);                
                var offset = new DateTimeOffset(nextDay);
                string key = $"{this.Bank}_{DateTime.UtcNow.ToString("yyyyMMdd")}";
                var mc = MemoryCache.Default;
                value = value
                    .Where(it => Math.Abs(it.Date.Subtract(now).TotalDays) < 1)
                    .ToArray();
                if (value.Any())
                    mc.Set(key, value, offset);
                    
            }
        }

    }
}
