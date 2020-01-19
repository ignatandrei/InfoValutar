using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoValutarLoadingLibs;
using InfoValutarShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoValutarWebAPI.Controllers
{
    /// <summary>
    /// try with version 1.0
    /// </summary>    
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ResponseCache(CacheProfileName = "Default30")]
    public class TodayRates : ControllerBase
    {
        private readonly ILogger<TodayRates> _logger;
        private readonly LoadExchangeProviders _prov;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="prov"></param>
        public TodayRates(ILogger<TodayRates> logger, LoadExchangeProviders prov)
        {
            _logger = logger;
            _prov = prov;
        }

        /// <summary>
        ///  all available banks
        ///  version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Banks()
        {
            return _prov.Banks();
        }
        /// <summary>
        ///  all exchange rates for banks
        ///  version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet("{bank}")]
        public IAsyncEnumerable<ExchangeRates> Rates([FromRoute] string bank)
        {
            return _prov.Rates(bank);
           
        }
    }
}
