using System;
using System.Threading;
using System.Threading.Tasks;
using BikeStoresApi.Models.Configuration;
using BikeStoresApi.Models.Contracts;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeStoresApi.Models.Repositories
{
    public partial class BikeStoresContext : DbContext, IBikeStoresContext
    {
        public BikeStoresContext()
        {
        }

        public BikeStoresContext(DbContextOptions<BikeStoresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<UserInfo> UsersInfo { get; set; }

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            BrandsConfig.SetEntityBuilder(modelBuilder);
            CategoryConfig.SetEntityBuilder(modelBuilder);
            CustomerConfig.SetEntityBuilder(modelBuilder);
            OrderConfig.SetEntityBuilder(modelBuilder);
            OrderItemConfig.SetEntityBuilder(modelBuilder);
            ProductConfig.SetEntityBuilder(modelBuilder);
            StaffConfig.SetEntityBuilder(modelBuilder);
            StockConfig.SetEntityBuilder(modelBuilder);
            StoreConfig.SetEntityBuilder(modelBuilder);
            UserInfoConfig.SetEntityBuilder(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
