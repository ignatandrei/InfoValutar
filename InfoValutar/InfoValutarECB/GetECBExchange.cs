using InfoValutarShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InfoValutarECB
{
    public class GetECBExchange: BankGetExchange
    {
        private readonly HttpClient httpClient;
        public GetECBExchange() : this(null)
        {

        }
        public GetECBExchange(HttpMessageHandler handler)
        {
            if (handler != null)
                httpClient = new HttpClient(handler, disposeHandler: false);
            else
                httpClient = new HttpClient();
        }

        public string Bank => "ECB";

        public async Task<IEnumerable<ExchangeRates>> GetActualRates()
        {
            var b = this as BankGetExchange;
            if (b.TodayFromCache != null)
                return b.TodayFromCache;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var ret = new List<ExchangeRates>();
            var xml = await httpClient.GetStringAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            //Console.WriteLine($"{xml}");
            var serializer = new XmlSerializer(typeof(Envelope));
            Envelope result;
            using (var reader = new StringReader(xml))
            {
                result = serializer.Deserialize(reader) as Envelope;
            }
            var val = result.Cube.Cube1;
            var date = val.time;
            string orig = "ECB";
            foreach (var item in val.Cube)
            {
                var exch = new ExchangeRates
                {
                    Bank = orig,
                    Date = date,
                    ExchangeTo = "EUR",
                    ExchangeFrom = item.currency,
                    ExchangeValue = item.rate
                };
                ret.Add(exch);
            }
            b.TodayFromCache = ret;

            return ret;


        }
    }
}

