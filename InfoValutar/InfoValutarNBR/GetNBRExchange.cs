using HtmlAgilityPack;
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
        public async Task<IEnumerable<ExchangeRates>> GetPreviousRates(DateTime dateTime)
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
            var val = result.Body.Cube.Where(it=>it.date.Subtract(dateTime).TotalDays<1).First();
            var ret = new List<ExchangeRates>();

            foreach (var item in val.Rate)
            {
                var exch = new ExchangeRates
                {
                    Bank = this.Bank,
                    Date = dateTime,
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
            return ret;
            //https://www.bnr.ro/files/xml/years/nbrfxrates2020.xml

            //                .Where(it => 
            //it.date.Subtract(dateTime).TotalDays > -1 &&
            //it.date.Subtract(dateTime).TotalDays < 1)

            //var html = await httpClient.GetStringAsync($"https://www.bnr.ro/files/xml/nbrfxrates{dateTime.Year}.htm");
            //var doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //var table = doc.DocumentNode.SelectNodes("//table").First();
            //var ret = new List<ExchangeRates>();
            //foreach(var item in table.SelectNodes("//thead/tr/th"))
            //{
            //    var ex = new ExchangeRates();
            //    ex.Bank = Bank;
            //    ex.ExchangeTo = "RON";
            //    ex.ExchangeTo = item.InnerText;
            //}
            return null;


        }
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
            
            foreach (var item in val.Rate)
            {
                var exch = new ExchangeRates
                {
                    Bank = this.Bank,
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

