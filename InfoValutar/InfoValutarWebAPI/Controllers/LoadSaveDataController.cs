using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfovalutarLoadAndSave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoValutarWebAPI.Controllers
{
    /// <summary>
    /// save data
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/save")]
    [ApiController]
    public class LoadSaveDataController : ControllerBase
    {
        /// <summary>
        /// load from all providers
        /// </summary>
        /// <param name="data">injected</param>
        /// <returns></returns>
        [HttpGet("LoadAndSaveAll")]
        public async Task<ResultsLoadBankData[]> LoadAndSaveAll([FromServices] LoadAndSaveLastData data)
        {
            return await data.LoadAndSave();
        }

    }
}