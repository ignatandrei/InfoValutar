using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InfoValutarShared
{
    public class GetNBRExchange
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async IAsyncEnumerable<ExchangeRates> GetActualRates()
        {
            var xml = await httpClient.GetStringAsync("https://www.bnr.ro/nbrfxrates.xml");
            //Console.WriteLine($"{xml}");
            var serializer = new XmlSerializer(typeof(DataSet));
            using (var reader = new StringReader(xml))
            {
                var result = serializer.Deserialize(reader) as DataSet;
                var val = result.Body.Cube.First();
                var date = val.date;
                string orig = "BNR";
                foreach (var item in val.Rate)
                {
                    var exch = new ExchangeRates();
                    exch.Bank = orig;
                    exch.date = date;
                    exch.ExchangeTo = "RON";
                    exch.ExchangeFrom = item.currency;
                    exch.ExchangeValue = item.Value;
                    if (!string.IsNullOrWhiteSpace(item.multiplier))
                    {
                        exch.ExchangeValue = exch.ExchangeValue / (decimal)int.Parse(item.multiplier);
                    }
                    yield return exch;
                }


            }
        }
    }
}
