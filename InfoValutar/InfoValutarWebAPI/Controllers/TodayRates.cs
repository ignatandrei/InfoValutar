using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoValutarLoadingLibs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoValutarWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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
            return _prov
                .LoadExchange()
                .Select(it=>it.Bank)
                .ToArray();
        }
    }
}
