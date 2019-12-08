using InfovalutarDB;
using InfoValutarLoadingLibs;
using InfoValutarShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfovalutarLoadAndSave
{
    
    public class ResultsLoadBankData
    {
        public string Bank { get; internal set; }
        public int NrRecordsLoaded { get; internal set; }
        public int NrRecordsSaved { get; internal set; }

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


            var items = providers.Banks().Select(it =>
                 new KeyValuePair<string, ResultsLoadBankData>(it,
                 new ResultsLoadBankData()
                 {
                     Bank = it,
                     ErrorMessage = "error loading",
                     HasSuccess = false,
                     NrRecordsSaved = 0,
                     NrRecordsLoaded = -1
                 })
             ); ;
            var lst = new Dictionary<string, ResultsLoadBankData>(items);


            var rates =
                providers.LoadExchange()
                .Select(it => it.GetActualRates())
                .ToArray();
            //TODO: how to load async all async enumerables?
            //TODO: how to report error if one fails?
            var allRates = new List<ExchangeRates>();
            foreach (var rateAsync in rates)
            {
                try
                {
                    var ratesBank = await rateAsync;
                    allRates.AddRange(ratesBank);
                }
                catch (Exception)
                {

                }
            }
            var groups = allRates.GroupBy(it => it.Bank).ToDictionary(it => it.Key, it => it.ToArray());
            foreach (var bank in groups.Keys)
            {
                var item = lst[bank];
                item.HasSuccess = true;
                item.ErrorMessage = null;
                item.NrRecordsLoaded = groups[bank].Length;
                item.NrRecordsSaved = 0;
                foreach (var rate in groups[bank])
                {

                    try
                    {

                        if (await ret.Exists(rate))
                            continue;
                        var nr = await save.Save(rate);
                        item.NrRecordsSaved++;
                    }
                    catch (Exception ex)
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
