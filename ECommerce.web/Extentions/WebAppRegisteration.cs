using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContext;
using ECommerce.Persistence.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.web.Extentions
{
    public static class WebAppRegisteration
    {

        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            

            var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dbContextService.Database.MigrateAsync();
            }
            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDbAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();


            var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dbContextService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var DataInitilizerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitilizer>("Default");
            await DataInitilizerService.InitilizeAsync();
            return app;
        }
        public static async Task<WebApplication> SeedIdentityDbAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var DataInitilizerService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitilizer>("Identity");
            await DataInitilizerService.InitilizeAsync();
            return app;
        }
    }
}
