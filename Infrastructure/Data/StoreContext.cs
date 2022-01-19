
using System;
using System.Linq;
using System.Reflection;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<Order> Orders { get; set; }  
        public DbSet<OrderItem> OrderItems { get; set; }  
          public DbSet<DeliveryMethod> DeliveryMethods { get; set; }  

        // by this we are overriding existing behaviour of creating entity. 
        // We are setting our own model config like required field , allow null, pkey
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

           modelBuilder.Entity<Order>().Property(p => p.Subtotal).HasColumnType("decimal(18,4)");

            // Added this as for sqllite only as sqllite does not provide decimal
            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                    var dateTimeProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset));
                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }

                    foreach (var property in dateTimeProperties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }
     }
}