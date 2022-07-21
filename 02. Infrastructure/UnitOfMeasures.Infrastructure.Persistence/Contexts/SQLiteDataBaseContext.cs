using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnitOfMeasures.Core.Domain.Dimension.Entities;
using UnitOfMeasures.Core.Domain.Unit.Entities;
using UnitOfMeasures.Infrastructure.Persistence.Configs;

namespace UnitOfMeasures.Infrastructure.Persistence.Contexts
{
    public class SQLiteDataBaseContext : DbContext
    {
        public DbSet<Core.Domain.Dimension.Entities.Dimension> Dimensions { get; set; }
        public DbSet<Core.Domain.Unit.Entities.Unit> Units { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = UnitOfMeasures.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
