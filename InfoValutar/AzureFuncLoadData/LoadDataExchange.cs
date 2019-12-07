using System;
using System.IO;
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
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            try
            {
                log.LogInformation($"%%% OK trigger function executed at: {DateTime.Now} next {myTimer.FormatNextOccurrences(1)} ");
                var folder = Path.Combine(context.FunctionDirectory, "plugins");
                log.LogInformation($"%%%  Folder {folder} Folder exists: {Directory.Exists(folder)}");
                
                folder = Path.Combine(context.FunctionAppDirectory, "plugins");
                log.LogInformation($"%%% Folder {folder} Folder exists: {Directory.Exists(folder)}");
                //var loader = new LoadExchangeProviders(folder);
                //var exchange = loader.LoadExchange().ToArray();
                //log.LogInformation($"%%% plugin number: {exchange.Length}");
            }
            catch (Exception ex)
            {
                log.LogError("ERROR! " + ex.Message );
                log.LogError("!!!" + ex.StackTrace);
                throw;
            }
        }
    }
}
