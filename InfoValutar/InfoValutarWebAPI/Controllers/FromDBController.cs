using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class FromDBController : ControllerBase
    {
        private readonly IRetrieve ret;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ret"></param>
        public FromDBController(IRetrieve ret)
        {
            this.ret = ret;
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
            return await ret.Rates(bank, from, to);
        }
    }
}