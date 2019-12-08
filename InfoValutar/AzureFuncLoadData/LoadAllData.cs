using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using InfoValutarLoadingLibs;
using System.Linq;

namespace AzureFuncLoadData
{
    public static class LoadAllData
    {
        [FunctionName("LoadAllData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                await Task.Delay(100);
                log.LogInformation($"### OK trigger function executed at: {DateTime.Now}  ");
                var folder = Path.Combine(context.FunctionDirectory, "plugins");
                log.LogInformation($"###  Folder {folder} Folder exists: {Directory.Exists(folder)}");

                folder = Path.Combine(context.FunctionAppDirectory, "plugins");
                log.LogInformation($"### Folder {folder} Folder exists: {Directory.Exists(folder)}");
                var loader = new LoadExchangeProviders(folder);
                var exchange = loader.LoadExchange().ToArray();
                log.LogInformation($"### plugin number: {exchange.Length}");
                return new OkObjectResult($"Hello");
            }
            catch (Exception ex)
            {
                log.LogError("ERROR! " + ex.Message);
                log.LogError("!!!" + ex.StackTrace);
                throw;
            }
            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
