﻿using InfoValutarShared;
using System.Threading.Tasks;

namespace InfovalutarDB
{
    public interface ISave
    {
        public Task<int> Save(params ExchangeRates[] er);        
    }
}
