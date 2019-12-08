using InfovalutarDB;
using InfoValutarNBR;
using InfoValutarShared;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace InfovalutarTest
{
    public class TestSaveAndRetrieve
    {
        [Fact]
        [Trait("External", "0")]
        public async Task SaveAndRetrieveBulk()
        {
            InMemoryDB.ResetInMemoryDatabase();
            InMemoryDB mem = new InMemoryDB(null);
            ISave s = new SaveSqlServer(mem);
            var response = await File.ReadAllTextAsync(Path.Combine("Data", "20191020bnr.txt"));
            var m = new MockHttpMessageHandler();
            m.When("https://www.bnr.ro/nbrfxrates.xml")
                .Respond("application/text", response);

            var nbr = new GetNBRExchange(m);
            var data = (await nbr.GetActualRates()).ToArray();
            var nr= await s.Save(data);
            Assert.Equal(nr, data.Length);
            var q = new RetrieveSqlServer(mem);
            var t = await q.Rates("BNR", DateTime.MinValue, DateTime.MaxValue);
            Assert.Equal(nr, t.Length);
        }
        [Fact]
        [Trait("External", "0")]
        public async Task SaveAndRetrieveOne()
        {
            InMemoryDB.ResetInMemoryDatabase();
            InMemoryDB mem = new InMemoryDB(null);
            ISave s = new SaveSqlServer(mem);
            var response = await File.ReadAllTextAsync(Path.Combine("Data", "20191020bnr.txt"));
            var m = new MockHttpMessageHandler();
            m.When("https://www.bnr.ro/nbrfxrates.xml")
                .Respond("application/text", response);

            var nbr = new GetNBRExchange(m);
            var data = (await nbr.GetActualRates()).ToArray();
            var     nr = await s.Save(data);
            var f = data[0];
            var q = new RetrieveSqlServer(mem);
            var t = await q.Rate("BNR",f.Date.Date,f. ExchangeFrom);
            Assert.Equal(f.ExchangeValue, t.ExchangeValue);
        }
    }
}
