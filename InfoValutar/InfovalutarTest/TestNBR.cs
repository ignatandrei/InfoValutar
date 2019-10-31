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
        public async Task TestLive()
        {
            var nbr = new GetNBRExchange();
            await foreach(var e in nbr.GetActualRates())
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
        public async Task TestParsing()
        {
            var response = await File.ReadAllTextAsync(Path.Combine("data", "20191020bnr.txt"));
            var m = new MockHttpMessageHandler();
            m.When("https://www.bnr.ro/nbrfxrates.xml")
                .Respond("application/text", response);

            var nbr = new GetNBRExchange(m);
            await foreach (var e in nbr.GetActualRates())
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
