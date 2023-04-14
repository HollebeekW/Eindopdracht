using Eindopdracht.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Identity.Client;
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
        public DbSet<ItemModel> Items { get; set; }
        public DbSet<AuthorModel> Authors { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MYDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>();
            modelBuilder.Entity<ItemModel>();
            modelBuilder.Entity<AuthorModel>();
            modelBuilder.Entity<RoleModel>();
            modelBuilder.Entity<CategoryModel>();
            base.OnModelCreating(modelBuilder);
        }

        public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
        {
            public MyDbContext CreateDbContext(string[]? args = null)
            {
                string defaultConnection = Settings.Default.ConnectionStrings;
                var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
                optionsBuilder.UseSqlServer(defaultConnection);

                return new MyDbContext(optionsBuilder.Options);
            }
        }

        public partial class UserViewModel
        {
            private readonly MyDbContext _dbContext;
            private readonly MyDbContextFactory _contextFactory;
            ObservableCollection<UserModel> _users;
            public ObservableCollection<UserModel> Users
            {
                get { return _users; }
                set { _users = value; }
            }

            public UserViewModel()
            {
                _contextFactory = new MyDbContextFactory();
                _dbContext = _contextFactory.CreateDbContext();
                _users = new ObservableCollection<UserModel>();
            }
        }

        public partial class AuthorViewModel
        {
            private readonly MyDbContext _dbContext;
            private readonly MyDbContextFactory _contextFactory;
            ObservableCollection<AuthorModel> _authors;
            public ObservableCollection<AuthorModel> Users
            {
                get { return _authors; }
                set { _authors = value; }
            }

            public AuthorViewModel()
            {
                _contextFactory = new MyDbContextFactory();
                _dbContext = _contextFactory.CreateDbContext();
                _authors = new ObservableCollection<AuthorModel>();
            }
        }
    }

}
