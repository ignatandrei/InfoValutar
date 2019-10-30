using InfoValutarShared;
using System;
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
    }
}
