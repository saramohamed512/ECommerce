
using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DataSeed;
using ECommerce.Persistence.Data.DbContext;
using ECommerce.Persistence.Repositories;
using ECommerce.ServiceAbstraction;
using ECommerce.Services;
using ECommerce.Services.MappingProfiles;
using ECommerce.web.Extentions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ECommerce.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataInitilizer, DataInitilizer>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new ProductProfile()));
            builder.Services.AddAutoMapper(X => X.AddProfile(new BasketProfile()));

            builder.Services.AddScoped<IProductService, ProductService>();


            builder.Services.AddSingleton<IConnectionMultiplexer>(O =>
            { 
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();

            var app = builder.Build();


            #region DataSeed

            await app.MigrateDbAsync();
            await app.SeedDbAsync();
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
