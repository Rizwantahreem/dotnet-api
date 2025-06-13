using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtension
{

    // With this code we can automate DB migration when application start running
    public static void MigrateDb(this WebApplication app)
    {

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }
}
