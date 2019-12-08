using System;
using InfoValutarNBR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFuncLoadData
{
    public static class LoadDataFromNBR
    {
        [FunctionName("LoadDataFromNBR")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            
            log.LogInformation($"LOAD NBR function processed a request: {DateTime.Now}");
            
            var nbr = new GetNBRExchange();
            var data = nbr.GetActualRates().GetAwaiter().GetResult();
            foreach (var item in data)
            {
                log.LogInformation($"1 {item.ExchangeFrom} = {item.ExchangeValue} {item.ExchangeTo}");

            }
        }
    }
}
