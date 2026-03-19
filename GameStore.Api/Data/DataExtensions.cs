using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{

    extension(WebApplication app)
    {
        // public static void MigrateDb(this WebApplication app)
        // {
        //     using var scope = app.Services.CreateScope();
        //     var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        //     dbContext.Database.Migrate();
        // }
        public void MigrateDb()
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            dbContext.Database.Migrate();
        }
    }

    extension(WebApplicationBuilder builder)
    {
        public void AddGameStoreDb()
        {
            var connectionStr = builder.Configuration.GetConnectionString("GameStore");

            // parameter in UseSeeding
            builder.Services.AddSqlite<GameStoreContext>(connectionStr,
            optionsAction: options => options.UseSeeding((context, _) =>
        {
            if (!context.Set<Genre>().Any())
            {
                context.Set<Genre>().AddRange(
                    // [
                    // new Genre { Name = "RPG" },
                    // new Genre { Name = "Fighting" },
                    // new Genre { Name = "Racing" }
                    // ]
                    //  list is not required?
                    new Genre { Name = "RPG" },
                    new Genre { Name = "Fighting" },
                    new Genre { Name = "Racing" }
                );
            }
            context.SaveChanges();
        }));
        }
    }

    // DTO is a mapper between API and DB?
    // WebAplication
    // Entity ORM?

}
