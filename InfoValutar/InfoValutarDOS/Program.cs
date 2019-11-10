using InfoValutarLoadingLibs;
using InfoValutarShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Terminal.Gui;

namespace InfoValutarDOS
{
    class Program
    {
        static async Task Main()
        {
            var loader = new LoadExchangeProviders("plugins");
            var exchange = loader.LoadExchange().ToArray();
            Console.WriteLine($"plugin number: {exchange.Length}");
            var l = new List<Task<ExchangeRates[]>>();
            for (int i = 0; i < exchange.Length; i++)
            {
                l.Add(Rates(exchange[i]));
            }

            var all = new List<ExchangeRates>();
            while (l.Count > 0)
            {
                var x = l.ToArray();

                var data = await Task.WhenAny(x);
                ShowValues(await data);
                all.AddRange(await data);
                l.Remove(data);
            }

            // in case of dotnet try, comment this line
            DisplayGui(all.ToArray());


        }
        static void DisplayGui(ExchangeRates[] exch)
        {
            var banks = exch.Select(it => it.Bank).Distinct().ToArray();
            Application.Init();
            var top = Application.Top;

            // Creates the top-level window to show
            var win = new Window("Exchange rates( press enter)")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            
            top.Add(win);
            for (int i = 0; i < banks.Length; i++)
            {
                var bank = banks[i];
                var b = new Button(10 * i, 3, bank)
                {
                    Clicked = () =>
                        MessageBox.Query(10, 10, bank,
                            string.Join(
                                '\n',
                                exch
                                .Where(it => it.Bank == bank)
                                .Select(e => $"1 {e.ExchangeFrom} = {e.ExchangeValue} {e.ExchangeTo}")
                                .ToArray()

                            ))
                };
                win.Add(b);

            }
            // Creates a menubar, the item "New" has a help menu.
            //var menu = new MenuBar(new MenuBarItem[] {
            //new MenuBarItem ("_File", new MenuItem [] {
            //    new MenuItem ("_New", "Creates new file", null),
            //    //new MenuItem ("_Close", "", () => Close ()),
            //    //new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
            //}),
            //new MenuBarItem ("_Edit", new MenuItem [] {
            //    new MenuItem ("_Copy", "", null),
            //    new MenuItem ("C_ut", "", null),
            //    new MenuItem ("_Paste", "", null)
            //})
        //});
            //top.Add(menu);
            Application.Run();
        }
        public static async Task<ExchangeRates[]> Rates(BankGetExchange bank)
        {
            var list = bank.GetActualRates();
            return await list.ToArrayAsync();
            
                        
        }
        public static void ShowValues(ExchangeRates[] list)
        {
            foreach (var e in list)
            {
                Console.WriteLine($"1 {e.ExchangeFrom} = {e.ExchangeValue} {e.ExchangeTo}");
            }
        }


    }
}
