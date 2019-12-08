using InfoValutarECB;
using InfoValutarNBR;
using InfoValutarShared;
using RichardSzalay.MockHttp;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace InfovalutarTest
{
    public class TestECB
    {
        [Fact]
        [Trait("External","1")]
        public async Task TestLive()
        {
            var ecb= new GetECBExchange();
            foreach(var e in await ecb.GetActualRates())
            {
                if(e.ExchangeTo == "RON")
                {
                    Assert.True(true);
                    return;
                }
            }
            Assert.True(false, "Should find RON");
        }
        [Fact]
        [Trait("External", "0")]
        public async Task TestParsing()
        {
            var response = await File.ReadAllTextAsync(Path.Combine("Data", "20191208bce.txt"));
            var m = new MockHttpMessageHandler();
            m.When("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml")
                .Respond("application/text", response);

            var bce = new GetECBExchange(m);
            foreach (var e in await bce.GetActualRates())
            {
                if (e.ExchangeTo == "RON")
                {
                    Assert.Equal(4.7800m, e.ExchangeValue, 4);
                    return;
                }
            }
            Assert.True(false, "Should find EUR");
        }
    }
}
