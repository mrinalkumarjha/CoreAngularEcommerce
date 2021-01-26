
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
    }
}