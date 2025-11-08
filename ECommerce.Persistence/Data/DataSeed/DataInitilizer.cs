using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistence.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataInitilizer : IDataInitilizer
    {
        private readonly StoreDbContext _dbContext;
        public DataInitilizer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initilize()
        {
            try
            {
                var HasProducts = _dbContext.Products.Any();
                var HasBrands = _dbContext.ProductBrands.Any();
                var HasTypes = _dbContext.ProductTypes.Any();
                if (HasProducts && HasBrands && HasTypes)
                {
                    return;
                }
        
                if (!HasBrands)
                    SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                if (!HasTypes)
                    SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                _dbContext.SaveChanges();
                if (!HasProducts)
                    SeedDataFromJson<Product, int>("products.json", _dbContext.Products);
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Data Seed Failed : {ex}");
            }
        }
        private void SeedDataFromJson<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            //FilePath
            //G:\Courses\.net Route\08 ASP.NET Core API\ECommerce.web.solution\ECommerce.Persistence\Data\DataSeed\JsonFiles\
            var FilePath = @"..\ECommerce.web.solution\ECommerce.Persistence\Data\DataSeed\JsonFiles\"+fileName;
            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {fileName} Not Found !");
            try
            {
                using var DataStream = File.OpenRead(FilePath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                if (Data is not null)
                {
                    dbset.AddRange(Data);
                   
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed to Read Data From JSON : {ex}");
            }
        }
    }
}
