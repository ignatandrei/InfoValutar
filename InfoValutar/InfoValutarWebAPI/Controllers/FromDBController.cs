using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using InfovalutarDB;
using InfoValutarShared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoValutarWebAPI.Controllers
{
    /// <summary>
    /// retrieve data from database
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/rates")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Default30")]
    public class FromDBController : ControllerBase
    {
        private readonly IRetrieve retrieve;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ret"></param>
        public FromDBController(IRetrieve ret)
        {
            this.retrieve = ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="fromDate">in the form yyyyMMdd</param>
        /// <param name="toDate">in the form yyyyMMdd</param>
        /// <returns></returns>
        [HttpGet("{bank}/{fromDate}/{toDate}")]
        public async Task<ExchangeRates[]> Rates([FromRoute] string bank, [FromRoute]string fromDate, [FromRoute]string toDate)
        {
            //TODO: return an error if not parseexact
            DateTime from = DateTime.ParseExact(fromDate, "yyyyMMdd", null);
            DateTime to = DateTime.ParseExact(toDate, "yyyyMMdd", null);
            var ret = await retrieve.Rates(bank, from, to);
            return ret;
            //return new[] { new ExchangeRates() { Bank = "ASD" } };
        }
        /// <summary>
        /// get rate
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        [HttpGet("{year}/{month}/{day}/{exchange}.{bank}")]
        [HttpGet("{bank}/{year}/{month}/{day}/{exchange}")]
        public async Task<string> RatesOnDate([FromRoute] string bank, 
            [FromRoute]int year, [FromRoute]int month,
            [FromRoute] int day, [FromRoute] string exchange)
        {
            //TODO: return an error if not correct year...
            DateTime dt = new DateTime(year, month, day);
            var ret=await retrieve.Rate(bank, dt, exchange);
            return ret.ExchangeValue.ToString();
            
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        [HttpGet("{bank}/azi/{exchange}")]
        public async Task<string> RatesToday([FromRoute] string bank,[FromRoute] string exchange)
        {
            await Task.Delay(100);
            DateTime dt = DateTime.Today;
            var ret= await retrieve.Rate(bank, dt, exchange);
            return ret.ExchangeValue.ToString();
            
            
        }
        /// <summary>
        /// Gets the rss feed
        /// </summary>
        /// <param name="bank"></param>
        /// <returns></returns>
        [HttpGet("{bank}/rss")]
        public async Task<IActionResult> GetRssFeed(string bank)
        {
            //TODO: move the logic somewhere where can be tested
            var data= await retrieve.TodayRates(bank);
            var items = data
                .Select(it =>
                new SyndicationItem(
                    it.ExchangeTo,
                    it.ExchangeValue.ToString(),
                    new Uri($"http://www.infovalutar.ro/bnr/{it.Date.Year}/{it.Date.Month}/{it.Date.Day}/{it.ExchangeTo}"))
                )
                .ToArray();

            var feed = new SyndicationFeed(
                "Curs Valutar", 
                "CursValutar, case, banci",
                new Uri( "http://www.infovalutar.ro/"),
                items
                );
            feed.Language = "ro-ro";
            feed.TimeToLive = TimeSpan.FromMinutes(10);
            using var sw = new StringWriter();
            using var rssWriter = XmlWriter.Create(sw);

            var rssFormatter = new Rss20FeedFormatter(feed,false);
            rssFormatter.WriteTo(rssWriter);
            rssWriter.Close();            
            return Content(sw.ToString(), "text/xml");
        }
    }
}