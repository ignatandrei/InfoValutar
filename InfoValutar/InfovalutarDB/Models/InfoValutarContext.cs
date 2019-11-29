using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InfovalutarDB.Models
{
    public partial class InfoValutarContext : DbContext
    {
        

        public InfoValutarContext(DbContextOptions<InfoValutarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ecb> Ecb { get; set; }
        public virtual DbSet<Nbr> Nbr { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ecb>(entity =>
            {
                entity.HasKey(e => new { e.ExchangeFrom, e.ExchangeTo, e.Date });

                entity.ToTable("ECB");

                entity.Property(e => e.ExchangeFrom).HasMaxLength(50);

                entity.Property(e => e.ExchangeTo).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ExchangeValue).HasColumnType("decimal(18, 6)");
            });

            modelBuilder.Entity<Nbr>(entity =>
            {
                entity.HasKey(e => new { e.ExchangeFrom, e.ExchangeTo, e.Date });

                entity.ToTable("NBR");

                entity.Property(e => e.ExchangeFrom).HasMaxLength(50);

                entity.Property(e => e.ExchangeTo).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ExchangeValue).HasColumnType("decimal(18, 6)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
