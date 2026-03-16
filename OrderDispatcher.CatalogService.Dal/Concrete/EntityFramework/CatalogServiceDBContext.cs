using Microsoft.EntityFrameworkCore;
using OrderDispatcher.CatalogService.Entities;

namespace OrderDispatcher.CatalogService.Dal.Concrete.EntityFramework
{
    public class CatalogServiceDBContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<StoreProduct> StoreProducts { get; set; }

        public CatalogServiceDBContext(DbContextOptions<CatalogServiceDBContext> options) : base(options)
        {
        }

        public CatalogServiceDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}


