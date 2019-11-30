using InfovalutarDB;
using InfoValutarLoadingLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfovalutarLoadAndSave
{
    
    public class ResultsLoadBankData
    {
        public string Bank { get; internal set; }
        public int NrRecords { get; internal set; }
        public bool HasSuccess { get; internal set; }
        public string ErrorMessage { get; internal set; }
    }
    /// <summary>
    /// questions at http://msprogrammer.serviciipeweb.ro/2019/12/19/saving-multiple-datapart-31/
    /// How we can display the errors ?
    /// What if the data exists already for this day and I cannot save into database, because it exists?
    /// How to acknowledge what exists and what not , in one operation ?
    /// How to report success even if data exists ? Should we report number of records?
    /// How we can perform async all that stuff and, however , report errors ?
    /// </summary>
    public class LoadAndSaveLastData
    {
        private readonly ISave save;
        private readonly IRetrieve ret;
        private readonly LoadExchangeProviders providers;
        public LoadAndSaveLastData(ISave save, IRetrieve ret)
        {
            providers = new LoadExchangeProviders("plugins");
            this.save = save;
            this.ret = ret;
        }

        public async Task<ResultsLoadBankData[]> LoadAndSave()
        {
            
            
            var items= providers.Banks().Select(it =>
                new KeyValuePair<string, ResultsLoadBankData>(it,
                new ResultsLoadBankData()
                {
                    Bank = it,
                    ErrorMessage=null,
                    HasSuccess=true,
                    NrRecords=0
                })
             );
            var lst = new Dictionary<string, ResultsLoadBankData>(items);


            var rates = 
                providers.LoadExchange()
                .Select(it => it.GetActualRates())
                .ToArray();
            //TODO: how to load async all async enumerables?
            //TODO: how to report error if one fails?
            foreach (var rateAsync in rates)
            {
                
                await foreach(var rate in rateAsync)
                {
                    var item = lst[rate.Bank];
                    try
                    {
                        
                        if (await ret.Exists(rate))
                            continue;
                        var nr = await save.Save(rate);
                        item.NrRecords++;
                    }
                    catch(Exception ex)
                    {
                        //TODO:log
                        item.ErrorMessage = ex.Message;
                        item.HasSuccess = false;
                    }
                }
                
                
            }
            return lst.Values.ToArray();
        }

    }
}
