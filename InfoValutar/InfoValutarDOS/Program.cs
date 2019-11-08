using InfoValutarLoadingLibs;
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
        static async Task Main()
        {
            var loader = new LoadExchangeProviders("plugins");
            var exchange = loader.LoadExchange().ToArray();
            Console.WriteLine($"plugin number: {exchange.Length}");
            var l = new List<Task<ExchangeRates[]>>();
            for (int i = 0; i < exchange.Length; i++)
            {
                l.Add(Rates(exchange[i]));
            }
            
            
            while (l.Count > 0)
            {
                var x = l.ToArray();

                var data = await Task.WhenAny(x);
                ShowValues(await data);
                l.Remove(data);
            }
        }
        public static async Task<ExchangeRates[]> Rates(BankGetExchange bank)
        {
            var list = bank.GetActualRates();
            return await list.ToArrayAsync();
            
                        
        }
        public static void ShowValues(ExchangeRates[] list)
        {
            foreach (var e in list)
            {
                Console.WriteLine($"1 {e.ExchangeFrom} = {e.ExchangeValue} {e.ExchangeTo}");
            }
        }


    }
}
