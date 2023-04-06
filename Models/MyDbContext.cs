using Eindopdracht.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eindopdracht.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            }

            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[]? args = null)
        {
            string defaultConnection = Settings.Default.ConnectionStrings;
            var OptionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            OptionsBuilder.UseSqlServer(defaultConnection);

            return new MyDbContext(OptionsBuilder.Options);
        }
    }
}
