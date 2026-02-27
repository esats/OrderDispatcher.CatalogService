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


            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(@"Server=.;database=OrderDP.CatalogDB;Trusted_Connection=True;MultipleActiveResultSets=true;Connect Timeout=150;TrustServerCertificate=True");
                //optionsBuilder.UseSqlServer(@"server=217.195.207.190;Initial Catalog=TexEx;User ID=Texexadmin;password=Texex2121**!!!");
            }

            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}


