using PrintToCash.AppData.Configuration.ConnectionString;
using PrintToCash.AppData.Entities;
using Microsoft.EntityFrameworkCore;

namespace PrintToCash.AppData
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Config> Configuration { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ConnectionString.MyConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(o => o.PricePaid).HasColumnType("decimal(8, 2)");
            modelBuilder.Entity<Product>().Property(o => o.Price).HasColumnType("decimal(8, 2)");
            modelBuilder.Entity<Material>().Property(o => o.Price).HasColumnType("decimal(8, 2)");
            modelBuilder.Entity<Config>().Property(o => o.CurrentCostElectricity).HasColumnType("decimal(7, 5)");
            modelBuilder.Entity<Config>().Property(o => o.FinalTouchHourlyFee).HasColumnType("decimal(4, 2)");

            modelBuilder.Entity<Config>().HasData(
            new Config { Id = 1, CurrentCostElectricity = 0.3M, FinalTouchHourlyFee = 0.5M }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
