using System;
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
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now} next {myTimer.FormatNextOccurrences(1)} ");
            log.LogInformation("Done");
        }
    }
}
