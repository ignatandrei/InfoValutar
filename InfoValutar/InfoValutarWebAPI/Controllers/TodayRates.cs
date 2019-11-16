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
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class TodayRates : ControllerBase
    {
        private readonly ILogger<TodayRates> _logger;
        private readonly LoadExchangeProviders _prov;

        public TodayRates(ILogger<TodayRates> logger, LoadExchangeProviders prov)
        {
            _logger = logger;
            _prov = prov;
        }

        [HttpGet]
        public IEnumerable<string> Banks()
        {
            return _prov.Banks();
        }
        [HttpGet("{bank}")]
        public IAsyncEnumerable<ExchangeRates> Rates([FromRoute] string bank)
        {
            return _prov.Rates(bank);
           
        }
    }
}
