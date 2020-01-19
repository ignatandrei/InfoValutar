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
        DateTime Key()
        {
            var now = DateTime.UtcNow;
            if (now.DayOfWeek == DayOfWeek.Sunday)
                now = now.AddDays(-1);
            if (now.DayOfWeek == DayOfWeek.Saturday)
                now = now.AddDays(-1);
            return now;

        }
        IEnumerable<ExchangeRates> TodayFromCache
        {
            get

            {
                var keyDate = Key();
                string key = $"{this.Bank}_{keyDate.ToString("yyyyMMdd")}";
                var mc = MemoryCache.Default;
                if (mc.Contains(key))
                    return mc[key] as ExchangeRates[];

                return null;

            }
            set
            {
                var keyDate = Key();
                string key = $"{this.Bank}_{keyDate.ToString("yyyyMMdd")}";
                var mc = MemoryCache.Default;
                value = value
                    .Where(it => Math.Abs(it.Date.Subtract(keyDate).TotalDays) < 1)
                    .ToArray();
                if (value.Any())
                {
                    mc.Set(key, value, DateTime.UtcNow.AddDays(7));
                   
                }
            }
        }

    }
}
