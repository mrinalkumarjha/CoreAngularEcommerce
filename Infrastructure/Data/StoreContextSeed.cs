using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    // For initial seeding data
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                using (var transaction = context.Database.BeginTransaction())
                {
                    // Seeding product brand
                    if (!context.ProductBrands.Any())
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductBrands ON;");
                        var brandData = File.ReadAllText(path + @"/Data/SeedData/brands.json");

                        // serialize brand json data into ProductBrands obj
                        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                        foreach (var item in brands)
                        {
                            context.ProductBrands.Add(item);
                        }
                        
                        await context.SaveChangesAsync();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductBrands OFF;");
                    }

                    // Seeding product types
                    if (!context.ProductTypes.Any())
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductTypes ON;");
                        var typesData = File.ReadAllText(path + @"/Data/SeedData/types.json");

                        // serialize brand json data into ProductType obj
                        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                        foreach (var item in types)
                        {
                            context.ProductTypes.Add(item);
                        }
                        await context.SaveChangesAsync();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductTypes OFF;");
                    }

                    //  // Seeding product 
                    if (!context.Products.Any())
                    {
                        var productData = File.ReadAllText(path + @"/Data/SeedData/products.json");

                        // serialize brand json data into ProductBrands obj
                        var products = JsonSerializer.Deserialize<List<Product>>(productData);
                        foreach (var item in products)
                        {
                            context.Products.Add(item);
                        }
                        await context.SaveChangesAsync();
                    }

                    //  // Seeding deliverymethod 
                    if (!context.DeliveryMethods.Any())
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.DeliveryMethods ON;");
                        var dmData = File.ReadAllText(path + @"/Data/SeedData/delivery.json");

                        // serialize brand json data into ProductBrands obj
                        var delMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                        foreach (var item in delMethods)
                        {
                            context.DeliveryMethods.Add(item);
                        }
                        await context.SaveChangesAsync();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.DeliveryMethods OFF;");
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                 var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                 logger.LogError(ex, "An error occured in seeding");
            }
        }
    }
}