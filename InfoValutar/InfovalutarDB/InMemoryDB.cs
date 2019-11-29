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
        private string GetConRead(string name)
        {
            return configuration?.GetConnectionString(name);
            

        }
        public static void ResetInMemoryDatabase()
        {            
            var cnt = new InfoValutarContext(new InMemoryDB(null).SqlOrMemory(null).Options);
            cnt.Database.EnsureDeleted();
        }
        internal DbContextOptionsBuilder<InfoValutarContext> SqlOrMemory(string name)
        {
            var ConnectionString = GetConRead(name);

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                opt = MemoryOptions();
            }
            else
            {
                opt = new DbContextOptionsBuilder<InfoValutarContext>();
                opt.UseSqlServer(ConnectionString);
            }
            return opt;
        }
        private DbContextOptionsBuilder<InfoValutarContext> MemoryOptions()
        {
            if (opt != null)
                return opt;

            opt = new DbContextOptionsBuilder<InfoValutarContext>();
            opt.UseInMemoryDatabase("test");//, new InMemoryDatabaseRoot());
            return opt;
        }

        
    }
}
