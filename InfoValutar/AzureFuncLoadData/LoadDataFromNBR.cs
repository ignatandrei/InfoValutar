using System;
using System.Linq;
using System.Threading.Tasks;
using InfovalutarDB;
using InfoValutarNBR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFuncLoadData
{
    public static class LoadDataFromNBR
    {
        [FunctionName("LoadDataFromNBR")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            
            log.Info($"LOAD NBR function processed a request: {DateTime.Now}");
            Console.WriteLine("------------------------!!");
            var nbr = new GetNBRExchange();
            var data = await nbr.GetActualRates();
            foreach (var item in data)
            {
                log.Info($"1 {item.ExchangeFrom} = {item.ExchangeValue} {item.ExchangeTo}");

            }
            log.Info("now saving to database");
            try
            {
                log.Info("trying to save");
                ISave save = new SaveSqlServer(null);
                await save.Save(data.ToArray());
            }
            catch(Exception ex)
            {
                log.Error($"ERROR !! {ex.Message}");
            }
        }
    }
}
