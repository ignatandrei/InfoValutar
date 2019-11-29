using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfovalutarDB.Models
{
    public partial class InfoValutarContext
    {
        public IQueryable<IExchangeRate> RateQ(string bank)
        {
            switch (bank?.ToLower())
            {
                case "ecb":
                    return this.Ecb.AsNoTracking();
                case "bnr":
                    return this.Nbr.AsNoTracking();
                default:
                    throw new ArgumentException($"cannot find bank {bank}");
            }
        }
    }
    public interface IExchangeRate
    {
        DateTime Date { get; set; }
        string ExchangeFrom { get; set; }
        string ExchangeTo { get; set; }
        decimal ExchangeValue { get; set; }
    }
    public partial class Ecb: IExchangeRate
    {
    }
    public partial class Nbr : IExchangeRate
    {
    }
}
