using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InfoValutarShared
{
    public class GetNBRExchange
    {
        private readonly HttpClient httpClient;

        public GetNBRExchange(HttpMessageHandler handler = null)
        {
            if (handler != null)
                httpClient = new HttpClient(handler, disposeHandler: false);
            else
                httpClient = new HttpClient();
        }
        public async IAsyncEnumerable<ExchangeRates> GetActualRates()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var xml = await httpClient.GetStringAsync("https://www.bnr.ro/nbrfxrates.xml");
            //Console.WriteLine($"{xml}");
            var serializer = new XmlSerializer(typeof(DataSet));
            DataSet result;
            using (var reader = new StringReader(xml))
            {
                result = serializer.Deserialize(reader) as DataSet;
            }
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

