using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoValutarWebAPI.Controllers
{
    /// <summary>
    /// info about commit
    /// </summary>
    public class LastCommitInfo
    {
        /// <summary>
        /// comment latest commit
        /// </summary>
        public string LatestCommit { get; set; }
        /// <summary>
        /// last date of commit
        /// </summary>
        public DateTime DateCommit { get; set; }
        /// <summary>
        /// last author of commit
        /// </summary>
        public string LastAuthor { get; set; }
    }
    /// <summary>
    /// controller about info the application
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/info")]
    [ResponseCache(CacheProfileName = "Default1Day")]
    public class InfoController
    {
        /// <summary>
        /// info about latest commit
        /// </summary>
        /// <returns></returns>
        public LastCommitInfo GetLatestCommit()
        {
            return
                new LastCommitInfo()
                {
                    LatestCommit = "{LatestCommit}",
                    DateCommit = DateTime.ParseExact("{DateCommit}", "yyyyMMdd:HHmmss", null),
                    LastAuthor = "{LastAuthor}"
                }
                ;
        }
    }
}
