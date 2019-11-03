using InfoValutarShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InfoValutarDOS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            await ShowValues(new GetNBRExchange());
            await ShowValues(new GetECBExchange());
        }
        public static async Task ShowValues(BankGetExchange bank)
        {
            var list = bank.GetActualRates();
            await foreach (var e in list)
            {
                Console.WriteLine($"1 {e.ExchangeFrom} = {e.ExchangeValue} {e.ExchangeTo}");
            }
        }


    }
}
