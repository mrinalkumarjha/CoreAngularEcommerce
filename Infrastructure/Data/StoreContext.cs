
using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // These package needed to use entityframework
    // Microsoft.EntityFrameworkCore    : varsion 3.1.1
    //Microsoft.EntityFrameworkCore.SqLite    : version 3.1.1
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } 
        public DbSet<ProductBrand> ProductBrands { get; set; }  
        public DbSet<ProductType> ProductTypes { get; set; }  

        // by this we are overriding existing behaviour of creating entity. 
        // We are setting our own model config like required field , allow null, pkey
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Added this as for sqllite only as sqllite does not provide decimal
            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }
    }
}