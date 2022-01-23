
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
    //Microsoft.EntityFrameworkCore.SqLServer   : version 3.1.1
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
        }
     }
}