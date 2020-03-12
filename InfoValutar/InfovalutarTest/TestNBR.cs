using InfoValutarNBR;
using InfoValutarShared;
using RichardSzalay.MockHttp;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace InfovalutarTest
{
    public class TestNBR
    {
        [Fact]
        [Trait("External","1")]
        public async Task TestLive()
        {
            var nbr = new GetNBRExchange();
            foreach(var e in await nbr.GetActualRates())
            {
                if(e.ExchangeFrom == "EUR")
                {
                    Assert.True(true);
                    return;
                }
            }
            Assert.True(false, "Should find EUR");
        }
        [Fact]
        [Trait("External", "1")]
        public async Task TestPrevData()
        {
            var date = new DateTime(2020, 03, 11);
            var nbr = new GetNBRExchange();
            foreach (var e in await nbr.GetPreviousRates(date))
            {
                if (e.ExchangeFrom == "EUR")
                {
                    Assert.True(true);
                    return;
                }
            }
            Assert.True(false, "Should find EUR");

        }

        [Fact]
        [Trait("External", "0")]
        public async Task TestParsing()
        {
            var response = await File.ReadAllTextAsync(Path.Combine("Data", "20191020bnr.txt"));
            var m = new MockHttpMessageHandler();
            m.When("https://www.bnr.ro/nbrfxrates.xml")
                .Respond("application/text", response);

            var nbr = new GetNBRExchange(m);
             foreach (var e in await nbr.GetActualRates())
            {
                if (e.ExchangeFrom == "EUR")
                {
                    Assert.Equal(4.7572m, e.ExchangeValue,4);
                    return;
                }
            }
            Assert.True(false, "Should find EUR");
        }
    }
}
