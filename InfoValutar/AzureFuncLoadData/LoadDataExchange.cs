using System;
using System.Linq;
using InfoValutarLoadingLibs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFuncLoadData
{
    public static class LoadDataExchange
    {
        [FunctionName("AllData")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now} next {myTimer.FormatNextOccurrences(1)} ");
                var loader = new LoadExchangeProviders("plugins");
                var exchange = loader.LoadExchange().ToArray();
                log.LogInformation($"plugin number: {exchange.Length}");
            }
            catch(Exception ex)
            {
                log.LogError("ERROR! " + ex.Message);
                throw;
            }
        }
    }
}
