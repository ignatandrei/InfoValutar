using InfovalutarDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfovalutarDB
{
    class InMemoryDB
    {
        DbContextOptionsBuilder<InfoValutarContext> opt;
        
        IConfigurationRoot configuration;
        private InMemoryDB()
        {
            configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        }
        internal string GetConRead(string name)
        {
            return configuration.GetConnectionString(name);
            

        }
        internal DbContextOptionsBuilder<InfoValutarContext> MemoryOptions()
        {
            if (opt != null)
                return opt;

            opt = new DbContextOptionsBuilder<InfoValutarContext>();
            opt.UseInMemoryDatabase("test");//, new InMemoryDatabaseRoot());
            return opt;
        }

        internal static InMemoryDB sing = new InMemoryDB();
    }
}
