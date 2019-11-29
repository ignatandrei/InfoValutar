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
        public async Task SaveAndRetrieve()
        {
            InMemoryDB mem = new InMemoryDB(null);
            ISave s = new SaveSqlServer(mem);
            var response = await File.ReadAllTextAsync(Path.Combine("Data", "20191020bnr.txt"));
            var m = new MockHttpMessageHandler();
            m.When("https://www.bnr.ro/nbrfxrates.xml")
                .Respond("application/text", response);

            var nbr = new GetNBRExchange(m);
            var data = await nbr.GetActualRates().ToArrayAsync();
            var nr= await s.Save(data);
            Assert.Equal(nr, data.Length);
            var q = new RetrieveSqlServer(mem);
            var t = await q.Rates("BNR", DateTime.MinValue, DateTime.MaxValue);
            Assert.Equal(nr, t.Length);
        }
    }
}
