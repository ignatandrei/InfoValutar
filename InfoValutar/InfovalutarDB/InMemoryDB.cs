using InfovalutarDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InfovalutarDB
{
    public class InMemoryDB
    {
        DbContextOptionsBuilder<InfoValutarContext> opt;
        IConfiguration configuration;
        public InMemoryDB(IConfiguration config)
        {
            this.configuration = config;
            
        }
        internal string GetConRead(string name)
        {
            return configuration?.GetConnectionString(name);
            

        }
        internal DbContextOptionsBuilder<InfoValutarContext> MemoryOptions()
        {
            if (opt != null)
                return opt;

            opt = new DbContextOptionsBuilder<InfoValutarContext>();
            opt.UseInMemoryDatabase("test");//, new InMemoryDatabaseRoot());
            return opt;
        }

        
    }
}
