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

namespace InfoValutarNBR
{
    public class GetNBRExchange: BankGetExchange
    {
        private readonly HttpClient httpClient;
        public GetNBRExchange() : this(null)
        {

        }
        public GetNBRExchange(HttpMessageHandler handler)
        {
            if (handler != null)
                httpClient = new HttpClient(handler, disposeHandler: false);
            else
                httpClient = new HttpClient();
        }

        public string Bank => "BNR";

        public async Task<IEnumerable<ExchangeRates>> GetActualRates()
        {
            var b = this as BankGetExchange;
            if (b.TodayFromCache != null)
                return b.TodayFromCache;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var ret = new List<ExchangeRates>();
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
                var exch = new ExchangeRates
                {
                    Bank = orig,
                    Date = date,
                    ExchangeTo = "RON",
                    ExchangeFrom = item.currency,
                    ExchangeValue = item.Value
                };
                if (!string.IsNullOrWhiteSpace(item.multiplier))
                {
                    exch.ExchangeValue /= (decimal)int.Parse(item.multiplier);
                }
                ret.Add(exch);
            }
            b.TodayFromCache = ret;

            return ret;


        }
    }
}

