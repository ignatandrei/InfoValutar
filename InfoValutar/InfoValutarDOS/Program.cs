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
            var nbr = new GetNBRExchange();
            var list = nbr.GetActualRates();
            await foreach (var e in list)
            {
                Console.WriteLine($"1 {e.ExchangeFrom} = {e.ExchangeValue} {e.ExchangeTo}");
            }

            var ecb = new GetECBExchange();
            list = ecb.GetActualRates();
            await foreach (var e in list)
            {
                Console.WriteLine($"1 {e.ExchangeFrom} = {e.ExchangeValue} {e.ExchangeTo}");
            }
        }


    }
}
