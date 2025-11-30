using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistence.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataInitilizer : IDataInitilizer
    {
        private readonly StoreDbContext _dbContext;
        public DataInitilizer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitilizeAsync()
        {
            try
            {
                var HasProducts =await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await  _dbContext.ProductTypes.AnyAsync();
                var HasDelivery= await _dbContext.Set<DeliveryMethod>().AnyAsync();


                if (HasProducts && HasBrands && HasTypes&& HasDelivery)
                {
                    return;
                }
        
                if (!HasBrands)
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                if (!HasTypes)
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                _dbContext.SaveChanges();
                if (!HasProducts)
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
               
                if (!HasDelivery)
                    await SeedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());

                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                // Log the error - in production, use ILogger instead of Console
                Console.WriteLine($"Data Seed Failed : {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                // Re-throw to make the error visible during development
                // Comment out the throw if you want silent failure in production
                throw;
            }
        }
        private async Task SeedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            // Get the base directory (usually bin/Debug/net8.0 or bin/Release/net8.0)
            var baseDirectory = AppContext.BaseDirectory;
            
            // Navigate to the solution root from the output directory
            // Path structure: bin/Debug/net8.0 -> bin/Debug -> bin -> ECommerce.web -> solution root
            var currentDir = new DirectoryInfo(baseDirectory);
            string? solutionRoot = null;
            
            // Try to find the solution root by looking for .sln file
            while (currentDir != null)
            {
                if (currentDir.GetFiles("*.sln").Any())
                {
                    solutionRoot = currentDir.FullName;
                    break;
                }
                currentDir = currentDir.Parent;
            }
            
            // If solution root not found, try navigating up 4 levels from base directory
            if (solutionRoot == null)
            {
                currentDir = new DirectoryInfo(baseDirectory);
                for (int i = 0; i < 4 && currentDir != null; i++)
                {
                    currentDir = currentDir.Parent;
                }
                solutionRoot = currentDir?.FullName;
            }
            
            // Build the path to JSON files
            string jsonFilesPath;
            if (solutionRoot != null && Directory.Exists(solutionRoot))
            {
                jsonFilesPath = Path.Combine(solutionRoot, "ECommerce.Persistence", "Data", "DataSeed", "JsonFiles", fileName);
            }
            else
            {
                // Fallback: try relative to base directory
                jsonFilesPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "ECommerce.Persistence", "Data", "DataSeed", "JsonFiles", fileName);
                jsonFilesPath = Path.GetFullPath(jsonFilesPath);
            }
            
            if (!File.Exists(jsonFilesPath))
            {
                throw new FileNotFoundException($"File {fileName} Not Found at {jsonFilesPath}!");
            }
            
            try
            {
                using var DataStream = File.OpenRead(jsonFilesPath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                if (Data is not null)
                {
                    await dbset.AddRangeAsync(Data);
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Read Data From JSON : {ex}");
                throw; // Re-throw to see the error
            }
        }
    }
}
