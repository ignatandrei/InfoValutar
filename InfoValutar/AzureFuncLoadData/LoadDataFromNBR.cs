using System;
using System.Linq;
using System.Net.Http;
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
        [FunctionName("LoadDataWebSiteBased")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            
            log.Info($"LOAD from website: {DateTime.Now}");
            Console.WriteLine("------------------------!!");
            var url = "https://infovalutar.azurewebsites.net/api/v1.0/save/LoadAndSaveAll";
            var http = new HttpClient();
            var data = await http.GetStringAsync(url);
            log.Info($"obtaining data {data}");

        }
    }
}
