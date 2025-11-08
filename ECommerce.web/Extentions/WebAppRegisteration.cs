using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContext;
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

        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var DataInitilizerService = scope.ServiceProvider.GetRequiredService<IDataInitilizer>();
            await DataInitilizerService.InitilizeAsync();
            return app;
        }
    }
}
