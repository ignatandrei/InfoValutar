using InfoValutarShared;
using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InfoValutarLoadingLibs
{
    public class LoadExchangeProviders
    {
        private readonly string folder;

        public LoadExchangeProviders(string folder)
        {
            this.folder = folder;
        }
        public async IAsyncEnumerable<ExchangeRates> Rates(string bank)
        {
            var provBank = LoadExchange()
                .FirstOrDefault(it => string.Equals(bank, it.Bank, StringComparison.InvariantCultureIgnoreCase));

            switch (provBank)
            {
                case null:
                    throw new ArgumentOutOfRangeException(nameof(bank),$"cannot find {bank}");
                    
                default:
                    {
                        await foreach (var data in provBank.GetActualRates())
                        {
                            yield return data;
                        }

                        break;
                    }
            }

        }
        public IEnumerable<string> Banks()
        {
            return
                LoadExchange()
                .Select(it => it.Bank)
                .ToArray();
        }
        public IEnumerable<BankGetExchange> LoadExchange()
        {
            var loaders = new List<PluginLoader>();
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, folder);
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        config => config.PreferSharedTypes = true
                        //sharedTypes: new[] { 
                        //    typeof(BankGetExchange),
                        //    typeof(ExchangeRates)
                        //}
                        );
                    loaders.Add(loader);
                }
            }

            // Create an instance of plugin types
            foreach (var loader in loaders)
            {
                foreach (var pluginType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(BankGetExchange).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    var plugin = Activator.CreateInstance(pluginType) as BankGetExchange;
                    yield return plugin;
                }
            }
        }
    }
}
